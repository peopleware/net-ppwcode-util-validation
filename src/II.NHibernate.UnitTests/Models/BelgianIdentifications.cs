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

using System.Collections.Generic;

using PPWCode.Util.Validation.II.European.Belgium;
using PPWCode.Util.Validation.II.NHibernate.European.Belgium;
using PPWCode.Vernacular.NHibernate.II.MappingByCode;
using PPWCode.Vernacular.Persistence.III;

namespace PPWCode.Util.Validation.II.NHibernate.UnitTests.Models
{
    public class BelgianIdentifications : PersistentObject<int>
    {
        public static IEqualityComparer<BelgianIdentifications> Comparer { get; }
            = new BelgianIdentificationsEqualityComparer();

        public virtual BBAN BBAN { get; set; }
        public virtual DMFA DMFA { get; set; }
        public virtual IBAN IBAN { get; set; }
        public virtual BIC BIC { get; set; }
        public virtual INSS INSS { get; set; }
        public virtual KBO KBO { get; set; }
        public virtual RSZ RSZ { get; set; }
        public virtual TemporaryRSZ TemporaryRSZ { get; set; }
        public virtual VAT VAT { get; set; }
        public virtual CompanyLocalUnitNumber CompanyLocalUnitNumber { get; set; }

        private sealed class BelgianIdentificationsEqualityComparer : IEqualityComparer<BelgianIdentifications>
        {
            public bool Equals(BelgianIdentifications x, BelgianIdentifications y)
            {
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x.GetType() != y.GetType())
                {
                    return false;
                }

                return Equals(x.BBAN, y.BBAN)
                       && Equals(x.DMFA, y.DMFA)
                       && Equals(x.IBAN, y.IBAN)
                       && Equals(x.BIC, y.BIC)
                       && Equals(x.INSS, y.INSS)
                       && Equals(x.KBO, y.KBO)
                       && Equals(x.RSZ, y.RSZ)
                       && Equals(x.TemporaryRSZ, y.TemporaryRSZ)
                       && Equals(x.VAT, y.VAT)
                       && Equals(x.CompanyLocalUnitNumber, y.CompanyLocalUnitNumber);
            }

            public int GetHashCode(BelgianIdentifications obj)
            {
                unchecked
                {
                    int hashCode = obj.BBAN != null ? obj.BBAN.GetHashCode() : 0;
                    hashCode = (hashCode * 397) ^ (obj.DMFA != null ? obj.DMFA.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.IBAN != null ? obj.IBAN.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.BIC != null ? obj.BIC.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.INSS != null ? obj.INSS.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.KBO != null ? obj.KBO.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.RSZ != null ? obj.RSZ.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.TemporaryRSZ != null ? obj.TemporaryRSZ.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.VAT != null ? obj.VAT.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.CompanyLocalUnitNumber != null ? obj.CompanyLocalUnitNumber.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }
    }

    public class BelgianIdentificationsMapper : PersistentObjectMapper<BelgianIdentifications, int>
    {
        public BelgianIdentificationsMapper()
        {
            Property(c => c.BBAN, m => m.Type<ValidBBAN>());
            Property(c => c.DMFA, m => m.Type<ValidDMFA>());
            Property(c => c.IBAN, m => m.Type<ValidIBAN>());
            Property(c => c.BIC, m => m.Type<ValidBIC>());
            Property(c => c.INSS, m => m.Type<ValidINSS>());
            Property(c => c.KBO, m => m.Type<ValidKBO>());
            Property(c => c.RSZ, m => m.Type<ValidRSZ>());
            Property(c => c.TemporaryRSZ, m => m.Type<ValidTemporaryRSZ>());
            Property(c => c.VAT, m => m.Type<ValidVAT>());
            Property(c => c.CompanyLocalUnitNumber, m => m.Type<ValidCompanyLocalUnitNumber>());
        }
    }
}
