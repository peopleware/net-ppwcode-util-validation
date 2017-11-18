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
using PPWCode.Util.Validation.I.European.Belgium;

namespace PPWCode.Util.Validation.I.UnitTests.European.Belgium
{
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Test")]
    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Test")]
    public class BBANTests : BaseTests
    {
        private static IEnumerable InvalidBBANs
        {
            get
            {
                yield return null;
                yield return string.Empty;
                yield return "1";
                yield return "12341234";
            }
        }

        private static IEnumerable StrictValidBBANs
        {
            get { yield return "850895676978"; }
        }

        private static IEnumerable ValidBBANs
        {
            get
            {
                foreach (object bban in StrictValidBBANs)
                {
                    yield return bban;
                }

                yield return "850895676978 ";
                yield return "850-8956769-78";
                yield return "BE 850895676978";
                yield return "BE 850-89.56.76-978";
            }
        }

        private static IEnumerable PaperVersions
        {
            get
            {
                yield return new TestCaseData("850895676978").Returns("850-8956769-78");
                yield return new TestCaseData("BE 850-89.56.76-978").Returns("850-8956769-78");
            }
        }

        [Test]
        [TestCaseSource(nameof(PaperVersions))]
        public string check_paperversion(string identification)
        {
            // Arrange
            BBAN bban = new BBAN(identification);

            // Act

            // Assert
            Assert.That(bban.IsValid, Is.True);
            Assert.That(bban.ElectronicVersion, Is.Not.Null);
            return bban.PaperVersion;
        }

        [Test]
        [TestCaseSource(nameof(InvalidBBANs))]
        public void bban_is_not_valid(string identification)
        {
            // Arrange
            BBAN bban = new BBAN(identification);

            // Act

            // Assert
            Assert.That(bban.IsValid, Is.False);
            Assert.That(bban.IsStrictValid, Is.False);
            Assert.That(bban.ElectronicVersion, Is.Null);
            Assert.That(bban.PaperVersion, Is.Null);
        }

        [Test]
        [TestCaseSource(nameof(StrictValidBBANs))]
        public void bban_is_strict_valid(string identification)
        {
            // Arrange
            BBAN bban = new BBAN(identification);

            // Act

            // Assert
            Assert.That(bban.IsValid, Is.True);
            Assert.That(bban.IsStrictValid, Is.True);
            Assert.That(bban.ElectronicVersion, Is.EqualTo(bban.CleanedVersion));
            Assert.That(bban.ElectronicVersion, Is.EqualTo(bban.RawVersion));
            Assert.That(bban.PaperVersion, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(nameof(ValidBBANs))]
        public void bban_is_valid(string identification)
        {
            // Arrange
            BBAN bban = new BBAN(identification);

            // Act

            // Assert
            Assert.That(bban.IsValid, Is.True);
            Assert.That(bban.ElectronicVersion, Is.Not.Null);
            Assert.That(bban.PaperVersion, Is.Not.Null);
        }
    }
}
