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

using System;
using NHibernate.Mapping.ByCode;
using PPWCode.Vernacular.NHibernate.I.Interfaces;
using PPWCode.Vernacular.NHibernate.I.MappingByCode;

namespace PPWCode.Util.Validation.I.NHibernate.UnitTests
{
    public class TestsSimpleModelMapper : SimpleModelMapper
    {
        public TestsSimpleModelMapper(IMappingAssemblies mappingAssemblies)
            : base(mappingAssemblies)
        {
        }

        protected override string DefaultSchemaName => @"dbo";

        protected override bool QuoteIdentifiers => true;

        protected override void OnBeforeMapProperty(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            base.OnBeforeMapProperty(modelInspector, member, propertyCustomizer);

            Type memberType = member.MemberType();
            if (typeof(AbstractIdentification).IsAssignableFrom(memberType))
            {
                AbstractIdentification identification = (AbstractIdentification) Activator.CreateInstance(memberType, string.Empty);
                propertyCustomizer.Length(identification.StandardMaxLength);
            }
        }
    }
}
