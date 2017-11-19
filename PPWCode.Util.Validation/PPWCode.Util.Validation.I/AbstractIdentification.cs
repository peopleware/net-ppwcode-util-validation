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

using System.Linq;
using System.Text;

namespace PPWCode.Util.Validation.I
{
    public abstract class AbstractIdentification : IIdentification
    {
        private string _cleanedVersion;
        private string _electronicVersion;
        private bool? _isStrictValid;
        private bool? _isValid;
        private string _paperVersion;

        protected AbstractIdentification(string rawVersion) => RawVersion = rawVersion;

        protected abstract string OnPaperVersion { get; }

        public abstract char PaddingCharacter { get; }

        protected virtual string OnElectronicVersion => CleanedVersion;

        public string CleanedVersion =>
            _cleanedVersion ?? (_cleanedVersion = Cleanup(RawVersion));

        public string ElectronicVersion =>
            _electronicVersion ?? (_electronicVersion = IsValid ? OnElectronicVersion : null);

        public bool IsStrictValid =>
            _isStrictValid ?? (bool) (_isStrictValid = Validate(RawVersion));

        public bool IsValid =>
            _isValid ?? (bool) (_isValid = Validate(CleanedVersion));

        public string PaperVersion =>
            _paperVersion ?? (_paperVersion = IsValid ? OnPaperVersion : null);

        public string RawVersion { get; }

        public virtual int StandardMaxLength => StandardMinLength;

        public abstract int StandardMinLength { get; }

        protected abstract bool OnValidate(string identification);

        protected virtual string Cleanup(string identification) => Pad(GetValidStream(identification));

        protected virtual bool Validate(string identification)
        {
            if (identification != null
                && StandardMinLength <= identification.Length
                && identification.Length <= StandardMaxLength)
            {
                return OnValidate(identification);
            }

            return false;
        }

        protected virtual string GetValidStream(string stream)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(stream.Length);
            foreach (char ch in stream.Where(IsValidChar))
            {
                sb.Append(ch);
            }

            return sb.ToString();
        }

        protected virtual bool IsValidChar(char ch) => char.IsDigit(ch);

        protected virtual string Pad(string identification) => identification?.PadLeft(StandardMaxLength, PaddingCharacter);
    }
}
