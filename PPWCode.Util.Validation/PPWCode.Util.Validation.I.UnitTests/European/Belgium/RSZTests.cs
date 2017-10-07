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
    public class RSZTests : BaseTests
    {
        private static IEnumerable InvalidRSZs
        {
            get
            {
                yield return null;
                yield return string.Empty;
                yield return "1";
                yield return "12341234";
            }
        }

        private static IEnumerable StrictValidRSZs
        {
            get
            {
                yield return "0133296720";
                yield return "5105009119";
            }
        }

        private static IEnumerable ValidRSZs
        {
            get
            {
                foreach (object kbo in StrictValidRSZs)
                {
                    yield return kbo;
                }

                yield return "133296720";
                yield return "RSZ 133296720";
                yield return "RSZ 0133296720";
                yield return "01332967.20";
            }
        }

        private static IEnumerable PaperVersions
        {
            get
            {
                yield return new TestCaseData("0133378103").Returns("01333781-03");
                yield return new TestCaseData("BE 0133378103").Returns("01333781-03");
            }
        }

        [Test]
        [TestCaseSource(nameof(PaperVersions))]
        public string check_paperversion(string identification)
        {
            // Arrange
            RSZ rsz = new RSZ(identification);

            // Act

            // Assert
            Assert.That(rsz.IsValid, Is.True);
            Assert.That(rsz.ElectronicVersion, Is.Not.Null);
            return rsz.PaperVersion;
        }

        [Test]
        [TestCaseSource(nameof(InvalidRSZs))]
        public void rsz_is_not_valid(string identification)
        {
            // Arrange
            RSZ rsz = new RSZ(identification);

            // Act

            // Assert
            Assert.That(rsz.IsValid, Is.False);
            Assert.That(rsz.IsStrictValid, Is.False);
            Assert.That(rsz.ElectronicVersion, Is.Null);
            Assert.That(rsz.PaperVersion, Is.Null);
        }

        [Test]
        [TestCaseSource(nameof(StrictValidRSZs))]
        public void rsz_is_strict_valid(string identification)
        {
            // Arrange
            RSZ rsz = new RSZ(identification);

            // Act

            // Assert
            Assert.That(rsz.IsValid, Is.True);
            Assert.That(rsz.IsStrictValid, Is.True);
            Assert.That(rsz.ElectronicVersion, Is.EqualTo(rsz.CleanedVersion));
            Assert.That(rsz.ElectronicVersion, Is.EqualTo(rsz.RawVersion));
            Assert.That(rsz.PaperVersion, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(nameof(ValidRSZs))]
        public void rsz_is_valid(string identification)
        {
            // Arrange
            RSZ rsz = new RSZ(identification);

            // Act

            // Assert
            Assert.That(rsz.IsValid, Is.True);
            Assert.That(rsz.ElectronicVersion, Is.Not.Null);
            Assert.That(rsz.PaperVersion, Is.Not.Null);
        }
    }
}
