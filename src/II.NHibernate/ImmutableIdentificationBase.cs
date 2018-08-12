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
using System.Data;
using System.Data.Common;

using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;

using PPWCode.Vernacular.Exceptions.III;
using PPWCode.Vernacular.NHibernate.II.Implementations;

namespace PPWCode.Util.Validation.II.NHibernate
{
    public abstract class ImmutableIdentificationBase<T> : ImmutableUserTypeBase
        where T : AbstractIdentification
    {
        public override Type ReturnedType
            => typeof(T);

        public override SqlType[] SqlTypes
            => new[]
               {
                   new SqlType(DbType.String)
               };

        protected abstract T Create(string identification);

        public override object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor sessionImplementor, object owner)
        {
            string identification = (string)NHibernateUtil.String.NullSafeGet(rs, names[0], sessionImplementor, owner);
            return identification == null ? null : Create(identification);
        }

        public override void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor sessionImplementor)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index, sessionImplementor);
                return;
            }

            T identification = (T)value;
            if (!identification.IsValid)
            {
                throw new ProgrammingError($"Trying to save TYPE: {typeof(T).Name} with INVALID VALUE: {identification.RawVersion}, only VALID identification is allowed.");
            }

            NHibernateUtil.String.NullSafeSet(cmd, identification.CleanedVersion, index, sessionImplementor);
        }
    }
}
