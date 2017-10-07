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
    public class INSSTests : BaseTests
    {
        private static IEnumerable InvalidINSSs
        {
            get
            {
                yield return null;
                yield return string.Empty;
                yield return "1";
                yield return "12341234";
            }
        }

        private static IEnumerable StrictValidINSSs
        {
            get
            {
                yield return "55250200801";
                yield return "01410191175";
                yield return "00030610329";
            }
        }

        private static IEnumerable ValidINSSs
        {
            get
            {
                foreach (object inss in StrictValidINSSs)
                {
                    yield return inss;
                }

                yield return "1410191175";
                yield return "30610329";
                yield return "BE 55250200801";
                yield return "BE 55.25.02 008-01";
            }
        }

        private static IEnumerable PaperVersions
        {
            get
            {
                yield return new TestCaseData("55250200801").Returns("55.25.02 008-01");
                yield return new TestCaseData("BE 55250200801").Returns("55.25.02 008-01");
            }
        }

        private static IEnumerable BisNumbers
        {
            get
            {
                foreach (object inss in InvalidINSSs)
                {
                    yield return new TestCaseData(inss).Returns(null);
                }

                yield return new TestCaseData("56252201101").Returns(true);
                yield return new TestCaseData("68500701501").Returns(true);
                yield return new TestCaseData("55250200801").Returns(true);
                yield return new TestCaseData("01410191175").Returns(true);
            }
        }

        [Test]
        [TestCaseSource(nameof(BisNumbers))]
        public bool? check_bis_numbers(string identification)
        {
            // Arrange
            INSS inss = new INSS(identification);

            // Act

            // Assert
            return inss.IsBisNumber;
        }

        [Test]
        [TestCaseSource(nameof(PaperVersions))]
        public string check_paperversion(string identification)
        {
            // Arrange
            INSS inss = new INSS(identification);

            // Act

            // Assert
            Assert.That(inss.IsValid, Is.True);
            Assert.That(inss.ElectronicVersion, Is.Not.Null);
            return inss.PaperVersion;
        }

        [Test]
        [TestCaseSource(nameof(InvalidINSSs))]
        public void inss_is_not_valid(string identification)
        {
            // Arrange
            INSS inss = new INSS(identification);

            // Act

            // Assert
            Assert.That(inss.IsValid, Is.False);
            Assert.That(inss.IsStrictValid, Is.False);
            Assert.That(inss.ElectronicVersion, Is.Null);
            Assert.That(inss.PaperVersion, Is.Null);
        }

        [Test]
        [TestCaseSource(nameof(StrictValidINSSs))]
        public void inss_is_strict_valid(string identification)
        {
            // Arrange
            INSS inss = new INSS(identification);

            // Act

            // Assert
            Assert.That(inss.IsValid, Is.True);
            Assert.That(inss.IsStrictValid, Is.True);
            Assert.That(inss.ElectronicVersion, Is.EqualTo(inss.CleanedVersion));
            Assert.That(inss.ElectronicVersion, Is.EqualTo(inss.RawVersion));
            Assert.That(inss.PaperVersion, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(nameof(ValidINSSs))]
        public void inss_is_valid(string identification)
        {
            // Arrange
            INSS inss = new INSS(identification);

            // Act

            // Assert
            Assert.That(inss.IsValid, Is.True);
            Assert.That(inss.ElectronicVersion, Is.Not.Null);
            Assert.That(inss.PaperVersion, Is.Not.Null);
        }
    }
}
