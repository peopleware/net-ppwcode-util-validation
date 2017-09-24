// Copyright 2017-2017 by PeopleWare n.v..
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
// 

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace PPWCode.Util.Validation.I.UnitTests
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Test")]
    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Test")]
    public class KBOTests : BaseTests
    {
        private static IEnumerable InvalidKBOs
        {
            get
            {
                yield return null;
                yield return string.Empty;
                yield return "1";
                yield return "12341234";
            }
        }

        private static IEnumerable StrictValidKBOs
        {
            get { yield return "0453834195"; }
        }

        private static IEnumerable ValidKBOs
        {
            get
            {
                foreach (var kbo in StrictValidKBOs)
                    yield return kbo;

                yield return "453834195";
                yield return "0453.834.195";
                yield return "BE 0453.834.195";
                yield return "BE 0453.834.195 Antwerp";
            }
        }

        private static IEnumerable PaperVersions
        {
            get { yield return new TestCaseData("0420936943").Returns("0420.936.943"); }
        }

        [Test, TestCaseSource(nameof(PaperVersions))]
        public string check_paperversion(string identification)
        {
            // Arrange
            var kbo = new KBO(identification);

            // Act

            // Assert
            Assert.That(kbo.IsValid, Is.True);
            Assert.That(kbo.ElectronicVersion, Is.Not.Null);
            return kbo.PaperVersion;
        }

        [Test, TestCaseSource(nameof(InvalidKBOs))]
        public void kbo_is_not_valid(string identification)
        {
            // Arrange
            var kbo = new KBO(identification);

            // Act

            // Assert
            Assert.That(kbo.IsValid, Is.False);
            Assert.That(kbo.IsStrictValid, Is.False);
            Assert.That(kbo.ElectronicVersion, Is.Null);
            Assert.That(kbo.PaperVersion, Is.Null);
        }

        [Test, TestCaseSource(nameof(StrictValidKBOs))]
        public void kbo_is_strict_valid(string identification)
        {
            // Arrange
            var kbo = new KBO(identification);

            // Act

            // Assert
            Assert.That(kbo.IsValid, Is.True);
            Assert.That(kbo.IsStrictValid, Is.True);
            Assert.That(kbo.ElectronicVersion, Is.EqualTo(kbo.CleanedVersion));
            Assert.That(kbo.ElectronicVersion, Is.EqualTo(kbo.RawVersion));
            Assert.That(kbo.PaperVersion, Is.Not.Null);
        }

        [Test, TestCaseSource(nameof(ValidKBOs))]
        public void kbo_is_valid(string identification)
        {
            // Arrange
            var kbo = new KBO(identification);

            // Act

            // Assert
            Assert.That(kbo.IsValid, Is.True);
            Assert.That(kbo.ElectronicVersion, Is.Not.Null);
            Assert.That(kbo.PaperVersion, Is.Not.Null);
        }
    }
}