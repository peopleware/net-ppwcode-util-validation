// Copyright 2017 by PeopleWare n.v..
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics.CodeAnalysis;

using NUnit.Framework;

using PPWCode.Util.Validation.II.European.Belgium;
using PPWCode.Util.Validation.II.NHibernate.UnitTests.Models;
using PPWCode.Util.Validation.II.NHibernate.UnitTests.Repositories;
using PPWCode.Vernacular.NHibernate.II.Interfaces;

namespace PPWCode.Util.Validation.II.NHibernate.UnitTests.IntegrationTests
{
    [Explicit]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Tests")]
    public class BelgianIdentificationsTests : BaseRepositoryTests<BelgianIdentifications>
    {
        protected override Func<ILinqRepository<BelgianIdentifications, int>> RepositoryFactory
            => () => new BelgianIdentificationsRepository(SessionProvider);

        [Test]
        public void can_handle_belgian_identifications()
        {
            // Arrange
            BelgianIdentifications subject =
                new BelgianIdentifications
                {
                    BBAN = new BBAN("850-895676-978"),
                    DMFA = new DMFA("DMFAP123456789A"),
                    INSS = new INSS("55.25.02.008-01"),
                    KBO = new KBO("0453.834.195"),
                    RSZ = new RSZ("0133-296 720"),
                    IBAN = new IBAN("AL472 1211 0090 0000 0023 5698 741"),
                    BIC = new BIC("KREDBEBB"),
                    TemporaryRSZ = new TemporaryRSZ("5 105009119"),
                    VAT = new VAT("0453.834.195"),
                    CompanyLocalUnitNumber = new CompanyLocalUnitNumber("2.069.315153")
                };

            // Act
            RunInsideTransaction(
                () =>
                {
                    Repository.SaveOrUpdate(subject);
                    Assert.That(subject.IsTransient, Is.False);
                },
                true);

            BelgianIdentifications actual = Repository.GetById(subject.Id);

            // Assert
            Assert.That(BelgianIdentifications.Comparer.Equals(subject, actual), Is.True);
        }
    }
}
