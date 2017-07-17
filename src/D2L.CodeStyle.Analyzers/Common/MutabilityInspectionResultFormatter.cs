﻿using System;

namespace D2L.CodeStyle.Analyzers.Common {
	public sealed class MutabilityInspectionResultFormatter {
		public string Format( MutabilityInspectionResult result ) {
			if( !result.IsMutable ) {
				return string.Empty;
			}

			var targetString = FormatTarget( result );
			var causeString = FormatCause( result );

			var formattedResult = $"{targetString} is {causeString}";
			return formattedResult;
		}

		private string FormatTarget( MutabilityInspectionResult result ) {
			switch( result.Target ) {
				case MutabilityTarget.Member:
					return $"'{result.MemberPath}'";
				case MutabilityTarget.Type:
					return $"'{result.MemberPath}''s type ('{result.TypeName}')";
				case MutabilityTarget.TypeArgument:
					return $"'{result.MemberPath}''s type argument ('{result.TypeName}')";
				default:
					throw new NotImplementedException( $"unknown target '{result.Target}'" );
			}
		}

		private string FormatCause( MutabilityInspectionResult result ) {
			switch( result.Cause ) {
				case MutabilityCause.IsAnArray:
					return "an array";
				case MutabilityCause.IsDynamic:
					return "dynamic";
				case MutabilityCause.IsAnInterface:
					return "an interface that is not marked with `[Objects.Immutable]`";
				case MutabilityCause.IsAnExternalUnmarkedType:
					return "defined in another assembly but is not marked with `[Objects.Immutable]`";
				case MutabilityCause.IsNotSealed:
					return "not sealed";
				case MutabilityCause.IsNotReadonly:
					return "not read-only";
				case MutabilityCause.IsPotentiallyMutable:
					return "not deterministically immutable";
				case MutabilityCause.IsADelegate:
					return "a delegate (which can hold onto mutable state)";
				default:
					throw new NotImplementedException( $"unknown cause '{result.Cause}'" );
			}
		}
	}
}
