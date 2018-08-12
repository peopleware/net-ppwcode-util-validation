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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PPWCode.Util.Validation.II
{
    [Serializable]
    [DataContract]
    public abstract class AbstractIdentification
        : IIdentification,
          IEquatable<AbstractIdentification>
    {
        [DataMember]
        private readonly string _rawVersion;

        [NonSerialized]
        private string _cleanedVersion;

        [NonSerialized]
        private string _cleanedVersionWithoutPadding;

        [NonSerialized]
        private string _electronicVersion;

        [NonSerialized]
        private bool? _isStrictValid;

        [NonSerialized]
        private bool? _isValid;

        [NonSerialized]
        private string _paperVersion;

        protected AbstractIdentification(string rawVersion)
        {
            _rawVersion = rawVersion;
        }

        protected abstract string OnPaperVersion { get; }

        public abstract char PaddingCharacter { get; }

        protected virtual string OnElectronicVersion
            => CleanedVersion;

        protected virtual string OnCleanedVersionWithoutPadding
            => GetValidStream(RawVersion);

        public bool Equals(AbstractIdentification other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(CleanedVersion, other.CleanedVersion);
        }

        public string CleanedVersionWithoutPadding
            => _cleanedVersionWithoutPadding ?? (_cleanedVersionWithoutPadding = OnCleanedVersionWithoutPadding);

        public string CleanedVersion
            => _cleanedVersion ?? (_cleanedVersion = Pad(CleanedVersionWithoutPadding));

        public string ElectronicVersion
            => _electronicVersion ?? (_electronicVersion = IsValid ? OnElectronicVersion : null);

        public bool IsStrictValid
            => _isStrictValid ?? (bool)(_isStrictValid = Validate(RawVersion));

        public bool IsValid
            => _isValid ?? (bool)(_isValid = Validate(CleanedVersion));

        public string PaperVersion
            => _paperVersion ?? (_paperVersion = IsValid ? OnPaperVersion : null);

        public string RawVersion
            => _rawVersion;

        public virtual int StandardMaxLength
            => StandardMinLength;

        public abstract int StandardMinLength { get; }

        protected abstract bool OnValidate(string identification);

        protected virtual bool Validate(string identification)
        {
            if ((identification != null)
                && (StandardMinLength <= identification.Length)
                && (identification.Length <= StandardMaxLength)
                && identification.All(IsValidChar))
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

        protected virtual bool IsValidChar(char ch)
            => char.IsDigit(ch);

        protected virtual string Pad(string identification)
            => identification?.PadLeft(StandardMaxLength, PaddingCharacter);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((AbstractIdentification)obj);
        }

        public override int GetHashCode()
            => CleanedVersion != null ? CleanedVersion.GetHashCode() : 0;

        public override string ToString()
            => PaperVersion ?? RawVersion;

        public static bool operator ==(AbstractIdentification left, AbstractIdentification right)
            => Equals(left, right);

        public static bool operator !=(AbstractIdentification left, AbstractIdentification right)
            => !Equals(left, right);
    }
}
