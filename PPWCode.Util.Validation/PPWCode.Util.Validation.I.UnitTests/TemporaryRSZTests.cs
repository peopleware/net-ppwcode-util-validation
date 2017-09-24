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
    public class TemporaryRSZTests : BaseTests
    {
        private static IEnumerable InvalidTemporaryRSZs
        {
            get
            {
                yield return null;
                yield return string.Empty;
                yield return "1";
                yield return "12341234";
            }
        }

        private static IEnumerable StrictValidTemporaryRSZs
        {
            get { yield return "5105009119"; }
        }

        private static IEnumerable ValidTemporaryRSZs
        {
            get
            {
                foreach (var kbo in StrictValidTemporaryRSZs)
                    yield return kbo;

                yield return "Temp. RSZ 5105009119";
                yield return "51050091.19";
            }
        }

        private static IEnumerable PaperVersions
        {
            get { yield return new TestCaseData("5105009119").Returns("51050091-19"); }
        }

        [Test, TestCaseSource(nameof(PaperVersions))]
        public string check_paperversion(string identification)
        {
            // Arrange
            var temporaryRSZ = new TemporaryRSZ(identification);

            // Act

            // Assert
            Assert.That(temporaryRSZ.IsValid, Is.True);
            Assert.That(temporaryRSZ.ElectronicVersion, Is.Not.Null);
            return temporaryRSZ.PaperVersion;
        }

        [Test, TestCaseSource(nameof(InvalidTemporaryRSZs))]
        public void temporary_rsz_is_not_valid(string identification)
        {
            // Arrange
            var temporaryRsz = new TemporaryRSZ(identification);

            // Act

            // Assert
            Assert.That(temporaryRsz.IsValid, Is.False);
            Assert.That(temporaryRsz.IsStrictValid, Is.False);
            Assert.That(temporaryRsz.ElectronicVersion, Is.Null);
            Assert.That(temporaryRsz.PaperVersion, Is.Null);
        }

        [Test, TestCaseSource(nameof(StrictValidTemporaryRSZs))]
        public void temporary_rsz_is_strict_valid(string identification)
        {
            // Arrange
            var temporaryRsz = new TemporaryRSZ(identification);

            // Act

            // Assert
            Assert.That(temporaryRsz.IsValid, Is.True);
            Assert.That(temporaryRsz.IsStrictValid, Is.True);
            Assert.That(temporaryRsz.ElectronicVersion, Is.EqualTo(temporaryRsz.CleanedVersion));
            Assert.That(temporaryRsz.ElectronicVersion, Is.EqualTo(temporaryRsz.RawVersion));
            Assert.That(temporaryRsz.PaperVersion, Is.Not.Null);
        }

        [Test, TestCaseSource(nameof(ValidTemporaryRSZs))]
        public void temporary_rsz_is_valid(string identification)
        {
            // Arrange
            var temporaryRsz = new TemporaryRSZ(identification);

            // Act

            // Assert
            Assert.That(temporaryRsz.IsValid, Is.True);
            Assert.That(temporaryRsz.ElectronicVersion, Is.Not.Null);
            Assert.That(temporaryRsz.PaperVersion, Is.Not.Null);
        }
    }
}