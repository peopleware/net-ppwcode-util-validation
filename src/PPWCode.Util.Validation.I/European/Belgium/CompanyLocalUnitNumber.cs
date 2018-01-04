// Copyright 2017 by PeopleWare n.v..
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace PPWCode.Util.Validation.I.European.Belgium
{
    [Serializable]
    [DataContract]
    public class CompanyLocalUnitNumber : KBO
    {
        private static readonly ISet<CompanyLocalUnitNumber> _validFictiveNumbers =
            new HashSet<CompanyLocalUnitNumber>
            {
                new CompanyLocalUnitNumber("8999999993"),
                new CompanyLocalUnitNumber("8999999104"),
                new CompanyLocalUnitNumber("8999999203"),
                new CompanyLocalUnitNumber("8999999302"),
                new CompanyLocalUnitNumber("8999999401"),
                new CompanyLocalUnitNumber("8999999005"),
                new CompanyLocalUnitNumber("8999999894")
            };

        /// <summary>
        ///     see
        ///     <see
        ///         href="https://www.socialsecurity.be/portail/glossaires/dmfa.nsf/ConsultFrImprPDF/BE5F2EE14FD71E23C1258194002DA178/$FILE/VersComplDMFA174_N.pdf" />
        ///     page 44.
        /// </summary>
        public CompanyLocalUnitNumber(string rawVersion)
            : base(rawVersion)
        {
        }

        protected override string OnPaperVersion
            => $"{CleanedVersion.Substring(0, 1)}.{CleanedVersion.Substring(1, 3)}.{CleanedVersion.Substring(4, 3)}.{CleanedVersion.Substring(7, 3)}";

        [ExcludeFromCodeCoverage]
        public override char PaddingCharacter
            => throw new InvalidOperationException();

        public static IEnumerable<CompanyLocalUnitNumber> ValidFictiveNumbers
            => _validFictiveNumbers;

        public bool IsFictiveNumber(string identification)
            => _validFictiveNumbers.Contains(new CompanyLocalUnitNumber(identification));

        protected override string Pad(string identification)
            => identification;

        protected override bool OnValidate(string identification)
        {
            if (new[] { '0', '1', '9' }.Contains(identification[0]))
            {
                return false;
            }

            return base.OnValidate(identification);
        }
    }
}
