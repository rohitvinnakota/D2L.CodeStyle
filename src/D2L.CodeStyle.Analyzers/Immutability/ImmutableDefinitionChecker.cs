﻿using System;
using System.Linq;
using D2L.CodeStyle.Analyzers.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D2L.CodeStyle.Analyzers.Immutability {
	internal sealed class ImmutableDefinitionChecker {
		private readonly SemanticModel m_model;
		private readonly Action<Diagnostic> m_diagnosticSink;
		private readonly ImmutabilityContext m_context;

		public ImmutableDefinitionChecker(
			SemanticModel model,
			Action<Diagnostic> diagnosticSink,
			ImmutabilityContext context
		) {
			m_model = model;
			m_diagnosticSink = diagnosticSink;
			m_context = context;
		}

		public bool CheckStruct( StructDeclarationSyntax decl ) => CheckDeclaration( decl );
		public bool CheckClass( ClassDeclarationSyntax decl ) => CheckDeclaration( decl );

		// This method is private to ensure callers only pass structs & classes
		private bool CheckDeclaration( TypeDeclarationSyntax decl ) {
			var result = true;

			var type = m_model.GetDeclaredSymbol( decl ) as ITypeSymbol;

			var members = type.GetMembers()
				// Exclude static members.
				// Immutability is, for us, a property held by values. A type
				// can be immutable in one of two ways and these are statements
				// about values of that type.
				// Static members are not tied to values/instances by
				// definition. Although we care about them otherwise, TODO explain
				.Where( m => !m.IsStatic );

			// Check that the base class is immutable
			var baseClassOk = m_context.IsImmutable(
				type.BaseType,
				ImmutableTypeKind.Instance,
				decl.BaseList?.Types.First()?.GetLocation(),
				out var diag
			);

			if( !baseClassOk ) {
				m_diagnosticSink( diag );
				result = false;
				// We can keep looking for more errors after this.
			}

			foreach( var member in members ) {
				// TODO: skip if it has Mutablity.Audited/Mutability.Unaudited.

				// TODO: if it does have one of the above but doesn't need it,
				//       error out (so we are forced to remove the annotation).

				if ( !CheckMember( member ) ) {
					result = false;
				}
			}

			return result;
		}

		private bool CheckMember( ISymbol member ) {
			switch( member.Kind ) {
				case SymbolKind.Field:
					return CheckField( member as IFieldSymbol );
				case SymbolKind.Property:
					return CheckProperty( member as IPropertySymbol );

				// These member types never contribute to mutability:
				case SymbolKind.Method:
				case SymbolKind.NamedType:
					return true;

				case SymbolKind.Event:
					m_diagnosticSink(
						Diagnostic.Create(
							Diagnostics.EventMemberMutable,
							member.GetDeclarationSyntax<MemberDeclarationSyntax>().GetLocation()
						)
					);

					return false;

				// By default raise an alarm (in case we missed something, or
				// if there are new unsupported language features.)
				default:
					m_diagnosticSink(
						Diagnostic.Create(
							Diagnostics.UnexpectedMemberKind,
							member.GetDeclarationSyntax<MemberDeclarationSyntax>().GetLocation(),
							member.Name,
							member.Kind
						)
					);

					return false;
			}
		}

		private bool CheckField( IFieldSymbol field ) {
			// These correspond to auto-properties. That case gets handled in
			// CheckProperty instead.
			if ( field.IsImplicitlyDeclared ) {
				return true;
			}

			var decl = field.GetDeclarationSyntax<VariableDeclaratorSyntax>();

			return CheckFieldOrProperty(
				decl,
				member: field,
				type: field.Type,
				isReadOnly: field.IsReadOnly,
				initializer: decl.Initializer?.Value
			);
		}


		private bool CheckProperty( IPropertySymbol prop ) {
			if ( prop.IsIndexer ) {
				// Indexer properties are just glorified method syntax and don't hold state.
				// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/
				return true;
			}

			var decl = prop.GetDeclarationSyntax<PropertyDeclarationSyntax>();

			if ( !decl.IsAutoImplemented() ) {
				// Properties that are auto-implemented have an implicit
				// backing field that may be mutable. Otherwise, properties are
				// just sugar for getter/setter methods and don't themselves
				// contribute to mutability.
				return true;
			}

			return CheckFieldOrProperty(
				decl,
				member: prop,
				type: prop.Type,
				isReadOnly: prop.IsReadOnly,
				initializer: decl.Initializer?.Value
			);
		}

		private bool CheckFieldOrProperty(
			SyntaxNode decl,
			ISymbol member,
			ITypeSymbol type,
			bool isReadOnly,
			ExpressionSyntax initializer
		) {
			var immutable = true;

			if ( !isReadOnly ) {
				m_diagnosticSink(
					Diagnostic.Create(
						Diagnostics.MemberIsNotReadOnly,
						decl.GetLocation(),
						member.Kind,
						member.Name,
						member.ContainingType.Name
					)
				);

				immutable = false;
			}

			// TODO: narrow inspection with initializer
			//       possibly a narrower type (using initializers type) and
			//       possibly ImmutableTypeKind.Instances if its a new T( ... ).

			if( !m_context.IsImmutable( type, ImmutableTypeKind.Total, decl.GetLocation(), out var diagnostic ) ) {
				m_diagnosticSink( diagnostic );
				immutable = false;
			}

			return immutable;
		}
	}
}