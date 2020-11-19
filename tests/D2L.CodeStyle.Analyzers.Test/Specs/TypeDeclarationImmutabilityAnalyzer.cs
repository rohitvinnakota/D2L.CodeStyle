﻿// analyzer: D2L.CodeStyle.Analyzers.Immutability.TypeDeclarationImmutabilityAnalyzer

using System;
using D2L.CodeStyle.Annotations;

#region Relevant Types

namespace D2L.CodeStyle.Annotations {
	public static class Objects {
		public abstract class ImmutableAttributeBase : Attribute {}
		public sealed class Immutable : ImmutableAttributeBase { }
		public sealed class ImmutableBaseClassAttribute : ImmutableAttributeBase { }
	}
	public static class Mutability {
		public sealed class AuditedAttribute : Attribute { }
		public sealed class UnauditedAttribute : Attribute {
			public UnauditedAttribute( Because why ) { }
		}
	}
	public enum Because {
		ItHasntBeenLookedAt = 1,
		ItsSketchy = 2,
		ItsStickyDataOhNooo = 3,
		WeNeedToMakeTheAnalyzerConsiderThisSafe = 4,
		ItsUgly = 5,
		ItsOnDeathRow = 6
	}
}

#endregion

namespace SpecTests {

	using static Immutable = Objects.Immutable;
	using static ImmutableBaseClass = Objects.ImmutableBaseClassAttribute;

	public sealed class Types {

		public enum SomeEnum {
			Foo
		}

		public interface RegularInterface { }

		[Immutable]
		public interface SomeImmutableInterface { }


		public class RegularClass {

			private static int m_field = 0;

			private static readonly int m_field = 0;

			private int m_field = 0;

			private readonly int m_field = 0;

		}
		public class RegularExtension : RegularClass { }
		public sealed class RegularSealedExtension : RegularClass { }

		[ImmutableBaseClass]
		public class SomeImmutableBaseClass { }
		static SomeImmutableBaseClass FuncReturningSomeImmutableBaseClass() => null;
		public sealed class MutableExtensionOfSomeImmutableBaseClass : SomeImmutableBaseClass { }

		[Immutable]
		public class SomeImmutableClass { }
		static SomeImmutableClass FuncReturningSomeImmutableClass() => null;

		[Immutable]
		public sealed class ClassMarkedImmutableImplementingRegularInterface : RegularInterface { }

		[ImmutableBaseClass]
		public class ClassMarkedImmutableBaseClassImplementingRegularInterface : RegularInterface { }

		public readonly struct SomeStruct { }

		[Immutable]
		public readonly struct SomeImmutableStruct { }

		public interface SomeGenericInterface<T, U> { }

		[Immutable]
		public interface SomeImmutableGenericInterface<T, U> { }

		[Immutable]
		public interface SomeImmutableGenericInterfaceGivenT<[Immutable] T, U> { }

		[Immutable]
		public interface SomeImmutableGenericInterfaceGivenU<T, [Immutable] U> { }

		[Immutable]
		public interface SomeImmutableGenericInterfaceGivenTU<[Immutable] T, [Immutable] U> { }

	}

	[Immutable]
	public sealed class AnalyzedClassMarkedImmutableExtendingMutableClass : /* NonImmutableTypeHeldByImmutable(Class, RegularClass,  (or [ImmutableBaseClass])) */ Types.RegularClass /**/ { }

	[ImmutableBaseClass]
	public sealed class AnalyzedClassMarkedImmutableBaseClassExtendingMutableClass : /* NonImmutableTypeHeldByImmutable(Class, RegularClass,  (or [ImmutableBaseClass])) */ Types.RegularClass /**/ { }

	[ImmutableBaseClass]
	public sealed class AnalyzedClassMarkedImmutableBaseClassExtendingImmutableBaseClass : Types.SomeImmutableBaseClass { }

	[Immutable]
	public sealed class AnalyzedClassMarkedImmutableExtendingImmutableBaseClass : Types.SomeImmutableBaseClass { }

	[Immutable]
	public sealed class AnalyzedClassMarkedImmutableExtendingImmutableClass : Types.SomeImmutableClass { }

	[Immutable]
	public sealed class AnalyzedClassMarkedImmutable {



		class SomeEventArgs { }
		delegate void SomeEventHandler( object sender, SomeEventArgs e );
		/* EventMemberMutable() */ event SomeEventHandler SomeEvent; /**/


		object this[ int index ] {
			get { return null };
			set { return; }
		}



		static int m_field = 0;
		static readonly int m_field = 0;
		int /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = 0;
		int /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		int m_field = 0;
		[Mutability.Audited]
		int m_field = 0;
		readonly int m_field = 0;
		[/* UnnecessaryMutabilityAnnotation() */ Mutability.Unaudited( Because.ItHasntBeenLookedAt ) /**/]
		readonly int m_field = 0;
		[/* UnnecessaryMutabilityAnnotation() */ Mutability.Audited /**/]
		readonly int m_field = 0;
		readonly int m_field;
		int Property { get; } = 0;
		int Property { get; }
		int Property { get { return 0; } }



		static Types.SomeEnum m_field = Types.SomeEnum.Foo;
		static readonly Types.SomeEnum m_field = Types.SomeEnum.Foo;
		Types.SomeEnum /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = Types.SomeEnum.Foo;
		Types.SomeEnum /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		Types.SomeEnum m_field = Types.SomeEnum.Foo;
		[Mutability.Audited]
		Types.SomeEnum m_field = Types.SomeEnum.Foo;
		readonly Types.SomeEnum m_field = Types.SomeEnum.Foo;
		readonly Types.SomeEnum m_field;
		Types.SomeEnum Property { get; } = Types.SomeEnum.Foo;
		Types.SomeEnum Property { get; }
		Types.SomeEnum Property { get { return Types.SomeEnum.Foo; } }



		static int[] m_field = new[] { 0 };
		static readonly int[] m_field = new[] { 0 };
		int[] /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = /* ArraysAreMutable(Int32) */ new[] { 0 } /**/;
		/* ArraysAreMutable(Int32) */ int[] /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		int[] m_field = new[] { 0 };
		[Mutability.Audited]
		int[] m_field = new[] { 0 };
		readonly int[] m_field = /* ArraysAreMutable(Int32) */ new[] { 0 } /**/;
		readonly /* ArraysAreMutable(Int32) */ int[] /**/ m_field;
		int[] Property { get; } = /* ArraysAreMutable(Int32) */ new[] { 0 } /**/;
		/* ArraysAreMutable(Int32) */ int[] /**/ Property { get; }
		int[] Property { get { return new[] { 0 }; } }



		static int* m_field;
		static readonly int* m_field;
		/* UnexpectedTypeKind(PointerType) */ int* /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		int* m_field;
		[Mutability.Audited]
		int* m_field;
		readonly /* UnexpectedTypeKind(PointerType) */ int* /**/ m_field;
		/* UnexpectedTypeKind(PointerType) */ int* /**/ Property { get; }
		int* Property { get { return new[] { 0 }; } }



		static object m_field;
		static readonly object m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ object /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Class, Object, ) */ object /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ object /**/ Property { get; }


		static object m_field = null;
		static readonly object m_field = null;
		object /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = null;
		readonly object m_field = null;
		object Property { get; } = null;
		object Property { get { return null; } }



		static object m_field = new object();
		static readonly object m_field = new object();
		object /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new object();
		readonly object m_field = new object();
		object Property { get; } = new object();
		object Property { get { return new object(); } }



		static dynamic m_field;
		static readonly dynamic m_field;
		/* DynamicObjectsAreMutable */ dynamic /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* DynamicObjectsAreMutable */ dynamic /**/ m_field;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		readonly dynamic m_field;
		[Mutability.Audited]
		readonly dynamic m_field;
		/* DynamicObjectsAreMutable */ dynamic /**/ Property { get; };
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		dynamic Property { get; }
		[Mutability.Audited]
		dynamic Property { get; }
		dynamic Property { get { return new ExpandoObject(); } }



		static Func<object> m_field = () => null;
		static readonly Func<object> m_field = () => null;
		Func<object> /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = () => null;
		readonly Func<object> m_field = () => null;
		Func<object> Property { get; } = () => null;
		Func<object> Property { get { return () => null; } }



		static Func<object> m_field = () => { return null };
		static readonly Func<object> m_field = () => { return null };
		Func<object> /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = () => { return null };
		readonly Func<object> m_field = () => { return null };
		Func<object> Property { get; } = () => { return null };
		Func<object> Property { get { return () => { return null }; } }



		static (int, int) m_field;
		static readonly (int, int) m_field;
		(int, int) /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly (int, int) m_field;
		(int, int) Property { get; }
		(int, int) Property { get { return default; } }



		static (object, int) m_field;
		static readonly (object, int) m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ (object, int) /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Class, Object, ) */ (object, int) /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ (object, int) /**/ Property { get; }
		(object, int) Property { get { return default; } }



		static (int, object) m_field;
		static readonly (int, object) m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ (int, object) /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly  /* NonImmutableTypeHeldByImmutable(Class, Object, ) */ (int, object) /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ (int, object) /**/ Property { get; }
		(int, object) Property { get { return default; } }



		static Types.RegularClass m_field = new Types.RegularClass();
		static readonly Types.RegularClass m_field = new Types.RegularClass();
		Types.RegularClass /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new /* NonImmutableTypeHeldByImmutable(Class, RegularClass,  (or [ImmutableBaseClass])) */ Types.RegularClass /**/ ();
		readonly Types.RegularClass m_field = new /* NonImmutableTypeHeldByImmutable(Class, RegularClass,  (or [ImmutableBaseClass])) */ Types.RegularClass /**/ ();
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		readonly Types.RegularClass m_field = new Types.RegularClass ();
		[Mutability.Audited]
		readonly Types.RegularClass m_field = new Types.RegularClass();
		Types.RegularClass Property { get; } = new /* NonImmutableTypeHeldByImmutable(Class, RegularClass,  (or [ImmutableBaseClass])) */ Types.RegularClass /**/ ();
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		Types.RegularClass Property { get; } = new Types.RegularClass ();
		[Mutability.Audited]
		Types.RegularClass Property { get; } = new Types.RegularClass();
		Types.RegularClass Property { get { return new Types.RegularClass(); } }



		static Types.RegularClass m_field = new Types.RegularExtension();
		static readonly Types.RegularClass m_field = new Types.RegularExtension();
		Types.RegularClass /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new /* NonImmutableTypeHeldByImmutable(Class, RegularExtension,  (or [ImmutableBaseClass])) */ Types.RegularExtension /**/ ();
		readonly Types.RegularClass m_field = new /* NonImmutableTypeHeldByImmutable(Class, RegularExtension,  (or [ImmutableBaseClass])) */ Types.RegularExtension /**/ ();
		Types.RegularClass Property { get; } = new /* NonImmutableTypeHeldByImmutable(Class, RegularExtension,  (or [ImmutableBaseClass])) */ Types.RegularExtension /**/ ();
		Types.RegularClass Property { get { return new Types.RegularExtension(); } }



		static Types.RegularClass m_field = new Types.RegularSealedExtension();
		static readonly Types.RegularClass m_field = new Types.RegularSealedExtension();
		Types.RegularClass /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new /* NonImmutableTypeHeldByImmutable(Class, RegularSealedExtension, ) */ Types.RegularSealedExtension /**/ ();
		readonly Types.RegularClass m_field = new /* NonImmutableTypeHeldByImmutable(Class, RegularSealedExtension, ) */ Types.RegularSealedExtension /**/ ();
		Types.RegularClass Property { get; } = new /* NonImmutableTypeHeldByImmutable(Class, RegularSealedExtension, ) */ Types.RegularSealedExtension /**/ ();
		Types.RegularClass Property { get { return new Types.RegularSealedExtension(); } }



		Types.SomeImmutableBaseClass /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new Types.SomeImmutableBaseClass();
		readonly Types.SomeImmutableBaseClass m_field = new Types.SomeImmutableBaseClass();
		Types.SomeImmutableBaseClass Property { get; } = new Types.SomeImmutableBaseClass();



		readonly Types.SomeImmutableBaseClass m_field = /* NonImmutableTypeHeldByImmutable(Class, SomeImmutableBaseClass, ) */ Types.FuncReturningSomeImmutableBaseClass() /**/;
		Types.SomeImmutableBaseClass Property { get; } = /* NonImmutableTypeHeldByImmutable(Class, SomeImmutableBaseClass, ) */ Types.FuncReturningSomeImmutableBaseClass() /**/;



		Types.SomeImmutableBaseClass /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new /* NonImmutableTypeHeldByImmutable(Class, MutableExtensionOfSomeImmutableBaseClass, ) */ Types.MutableExtensionOfSomeImmutableBaseClass /**/ ();
		readonly Types.SomeImmutableBaseClass m_field = new /* NonImmutableTypeHeldByImmutable(Class, MutableExtensionOfSomeImmutableBaseClass, ) */ Types.MutableExtensionOfSomeImmutableBaseClass /**/ ();
		Types.SomeImmutableBaseClass Property { get; } = new /* NonImmutableTypeHeldByImmutable(Class, MutableExtensionOfSomeImmutableBaseClass, ) */ Types.MutableExtensionOfSomeImmutableBaseClass /**/ ();



		Types.SomeImmutableClass /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new Types.SomeImmutableClass();
		readonly Types.SomeImmutableClass m_field = new Types.SomeImmutableClass();
		Types.SomeImmutableClass Property { get; } = new Types.SomeImmutableClass();



		readonly Types.SomeImmutableClass m_field = Types.FuncReturningSomeImmutableClass();
		Types.SomeImmutableClass Property { get; } = Types.FuncReturningSomeImmutableClass();



		static Types.RegularInterface m_field;
		static readonly Types.RegularInterface m_field;
		/* NonImmutableTypeHeldByImmutable(Interface, RegularInterface, ) */ Types.RegularInterface /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Interface, RegularInterface, ) */ Types.RegularInterface /**/ m_field;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		readonly Types.RegularInterface m_field;
		[Mutability.Audited]
		readonly Types.RegularInterface m_field;
		/* NonImmutableTypeHeldByImmutable(Interface, RegularInterface, ) */ Types.RegularInterface /**/ Property { get; }
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		Types.RegularInterface Property { get; }
		[Mutability.Audited]
		Types.RegularInterface Property { get; }



		static Types.RegularInterface m_field = new Types.ClassMarkedImmutableImplementingRegularInterface();
		static readonly Types.RegularInterface m_field = new Types.ClassMarkedImmutableImplementingRegularInterface();
		Types.RegularInterface /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new Types.ClassMarkedImmutableImplementingRegularInterface();
		readonly Types.RegularInterface m_field = new Types.ClassMarkedImmutableImplementingRegularInterface();
		Types.RegularInterface Property { get; } = new Types.ClassMarkedImmutableImplementingRegularInterface();



		static Types.RegularInterface m_field = new Types.ClassMarkedImmutableBaseClassImplementingRegularInterface();
		static readonly Types.RegularInterface m_field = new Types.ClassMarkedImmutableBaseClassImplementingRegularInterface();
		Types.RegularInterface /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new Types.ClassMarkedImmutableBaseClassImplementingRegularInterface();
		readonly Types.RegularInterface m_field = new Types.ClassMarkedImmutableBaseClassImplementingRegularInterface();
		Types.RegularInterface Property { get; } = new Types.ClassMarkedImmutableBaseClassImplementingRegularInterface();



		static Types.SomeStruct m_field;
		static readonly Types.SomeStruct m_field;
		/* NonImmutableTypeHeldByImmutable(Struct, SomeStruct, ) */ Types.SomeStruct /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		Types.SomeStruct /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/ = new /* NonImmutableTypeHeldByImmutable(Struct, SomeStruct, ) */ Types.SomeStruct /**/ ();
		readonly /* NonImmutableTypeHeldByImmutable(Struct, SomeStruct, ) */ Types.SomeStruct /**/ m_field;
		readonly Types.SomeStruct  m_field = new /* NonImmutableTypeHeldByImmutable(Struct, SomeStruct, ) */ Types.SomeStruct /**/ ();
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		readonly Types.SomeStruct m_field;
		[Mutability.Audited]
		readonly Types.SomeStruct m_field;
		/* NonImmutableTypeHeldByImmutable(Struct, SomeStruct, ) */ Types.SomeStruct /**/ Property { get; }
		Types.SomeStruct Property { get; } = new /* NonImmutableTypeHeldByImmutable(Struct, SomeStruct, ) */ Types.SomeStruct /**/ ();
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		Types.SomeStruct Property { get; }
		[Mutability.Audited]
		Types.SomeStruct Property { get; }



		static Types.SomeImmutableStruct m_field;
		static readonly Types.SomeImmutableStruct m_field;
		Types.SomeImmutableStruct /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly Types.SomeImmutableStruct m_field;
		Types.SomeImmutableStruct Property { get; }



		static Types.SomeGenericInterface<int, int> m_field;
		static readonly Types.SomeGenericInterface<int, int> m_field;
		/* NonImmutableTypeHeldByImmutable(Interface, SomeGenericInterface, ) */ Types.SomeGenericInterface<int, int> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Interface, SomeGenericInterface, ) */ Types.SomeGenericInterface<int, int> /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Interface, SomeGenericInterface, ) */ Types.SomeGenericInterface<int, int> /**/ Property { get; }
		Types.SomeGenericInterface<int, int> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenT<int, object> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenT<int, object> m_field;
		Types.SomeImmutableGenericInterfaceGivenT<int, object> /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly Types.SomeImmutableGenericInterfaceGivenT<int, object> m_field;
		Types.SomeImmutableGenericInterfaceGivenT<int, object> Property { get; }
		Types.SomeImmutableGenericInterfaceGivenT<int, object> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenT<object, int> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenT<object, int> m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenT<object, int> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenT<object, int> /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenT<object, int> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenT<object, int> Property { get { return default; } }


		static Types.SomeImmutableGenericInterfaceGivenU<object, int> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenU<object, int> m_field;
		Types.SomeImmutableGenericInterfaceGivenU<object, int> /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly Types.SomeImmutableGenericInterfaceGivenU<object, int> m_field;
		Types.SomeImmutableGenericInterfaceGivenU<object, int> Property { get; }
		Types.SomeImmutableGenericInterfaceGivenU<object, int> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenU<int, object> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenU<int, object> m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenU<int, object> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenU<int, object> /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenU<int, object> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenU<int, object> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenTU<int, int> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenTU<int, int> m_field;
		Types.SomeImmutableGenericInterfaceGivenTU<int, int> /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly Types.SomeImmutableGenericInterfaceGivenTU<int, int> m_field;
		Types.SomeImmutableGenericInterfaceGivenTU<int, int> Property { get; }
		Types.SomeImmutableGenericInterfaceGivenTU<int, int> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenTU<int, object> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenTU<int, object> m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenTU<int, object> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenTU<int, object> /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenTU<int, object> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenTU<int, object> Property { get { return default; } }


		static Types.SomeImmutableGenericInterfaceGivenTU<object, int> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenTU<object, int> m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenTU<object, int> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedClassMarkedImmutable) */ m_field /**/;
		readonly /* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenTU<object, int> /**/ m_field;
		/* NonImmutableTypeHeldByImmutable(Class, Object, ) */ Types.SomeImmutableGenericInterfaceGivenTU<object, int> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenTU<object, int> Property { get { return default; } }
	}

	[Immutable]
	public sealed class AnalyzedImmutableGenericClassGivenT<[Immutable] T, U> {



		static T m_field;
		static readonly T m_field;
		T /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly T m_field;
		T Property { get; }
		T Property { get { return default; } }



		static T m_field = new T();
		static readonly T m_field = new T();
		T /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/ = new T();
		readonly T m_field = new T();
		T Property { get; } = new T()
		T Property { get { return new T(); } }



		static U m_field;
		static readonly U m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ U /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		U m_field;
		[Mutability.Audited]
		U m_field;
		readonly /* TypeParameterIsNotKnownToBeImmutable(U) */ U /**/ m_field;
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		readonly U m_field;
		[Mutability.Audited]
		readonly U m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ U /**/ Property { get; }
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		U Property { get; }
		[Mutability.Audited]
		U Property { get; }
		U Property { get { return default; } }



		static U m_field = new U();
		static readonly U m_field = new U();
		U /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/ = new /* TypeParameterIsNotKnownToBeImmutable(U) */ U /**/();
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		U m_field = new U();
		[Mutability.Audited]
		U m_field = new U();
		readonly U m_field = new /* TypeParameterIsNotKnownToBeImmutable(U) */ U /**/ ();
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		readonly U m_field = new U();
		[Mutability.Audited]
		readonly U m_field = new U();
		U Property { get; } = new /* TypeParameterIsNotKnownToBeImmutable(U) */ U /**/ ();
		[Mutability.Unaudited( Because.ItHasntBeenLookedAt )]
		U Property { get; } = new U();
		[Mutability.Audited]
		U Property { get; } = new U();



		static Types.SomeImmutableGenericInterfaceGivenT<T, U> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenT<T, U> m_field;
		Types.SomeImmutableGenericInterfaceGivenT<T, U> /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly Types.SomeImmutableGenericInterfaceGivenT<T, U> m_field;
		Types.SomeImmutableGenericInterfaceGivenT<T, U> Property { get; }
		Types.SomeImmutableGenericInterfaceGivenT<T, U> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenT<U, T> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenT<U, T> m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenT<U, T> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly /* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenT<U, T> /**/ m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenT<U, T> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenT<U, T> Property { get { return default; } }


		static Types.SomeImmutableGenericInterfaceGivenU<U, T> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenU<U, T> m_field;
		Types.SomeImmutableGenericInterfaceGivenU<U, T> /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly Types.SomeImmutableGenericInterfaceGivenU<U, T> m_field;
		Types.SomeImmutableGenericInterfaceGivenU<U, T> Property { get; }
		Types.SomeImmutableGenericInterfaceGivenU<U, T> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenU<T, U> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenU<T, U> m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenU<T, U> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly /* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenU<T, U> /**/ m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenU<T, U> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenU<T, U> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenTU<T, T> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenTU<T, T> m_field;
		Types.SomeImmutableGenericInterfaceGivenTU<T, T> /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly Types.SomeImmutableGenericInterfaceGivenTU<T, T> m_field;
		Types.SomeImmutableGenericInterfaceGivenTU<T, T> Property { get; }
		Types.SomeImmutableGenericInterfaceGivenTU<T, T> Property { get { return default; } }



		static Types.SomeImmutableGenericInterfaceGivenTU<T, U> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenTU<T, U> m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenTU<T, U> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly /* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenTU<T, U> /**/ m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenTU<T, U> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenTU<T, U> Property { get { return default; } }


		static Types.SomeImmutableGenericInterfaceGivenTU<U, T> m_field;
		static readonly Types.SomeImmutableGenericInterfaceGivenTU<U, T> m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenTU<U, T> /**/ /* MemberIsNotReadOnly(Field, m_field, AnalyzedImmutableGenericClassGivenT) */ m_field /**/;
		readonly /* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenTU<U, T> /**/ m_field;
		/* TypeParameterIsNotKnownToBeImmutable(U) */ Types.SomeImmutableGenericInterfaceGivenTU<U, T> /**/ Property { get; }
		Types.SomeImmutableGenericInterfaceGivenTU<U, T> Property { get { return default; } }
	}

}