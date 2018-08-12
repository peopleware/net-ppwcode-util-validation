// Copyright 2018 by PeopleWare n.v..
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

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace PPWCode.Util.Validation.II.European.France
{
    /// <summary>
    ///     see <see href="https://fr.wikipedia.org/wiki/Num%C3%A9ro_de_s%C3%A9curit%C3%A9_sociale_en_France#ancrage_C" />
    /// </summary>
    [Serializable]
    [DataContract]
    public class NIR
        : AbstractFrIdentification,
          INationalNumberIdentification
    {
        private ParseResult _parseResult;

        public NIR(string rawVersion)
            : base(rawVersion)
        {
        }

        protected override string OnPaperVersion
            => $"{CleanedVersion.Substring(0, 1)} {CleanedVersion.Substring(1, 2)} {CleanedVersion.Substring(3, 2)} {CleanedVersion.Substring(5, 5)} {CleanedVersion.Substring(10, 3)} {CleanedVersion.Substring(13, 2)}";

        public override char PaddingCharacter
            => '0';

        public Sexe? Sexe
        {
            get
            {
                if (_parseResult == null)
                {
                    _parseResult = ParseINSEE();
                }

                return _parseResult.Sexe;
            }
        }

        public override int StandardMinLength
            => 15;

        public DateTime? BirthDate
        {
            get
            {
                if (_parseResult == null)
                {
                    _parseResult = ParseINSEE();
                }

                return _parseResult.BirthDate;
            }
        }

        private ParseResult ParseINSEE()
        {
            DateTime? birthdate = null;
            Sexe? sexe = null;

            if (IsValid)
            {
                switch (int.Parse(CleanedVersion.Substring(0, 1)))
                {
                    case 1:
                    case 3:
                    case 7:
                        sexe = II.Sexe.MALE;
                        break;

                    case 2:
                    case 4:
                    case 8:
                        sexe = II.Sexe.FEMALE;
                        break;

                    default:
                        sexe = II.Sexe.NOT_KNOWN;
                        break;
                }

                int yy = 1900 + int.Parse(CleanedVersion.Substring(1, 2));
                int mm = int.Parse(CleanedVersion.Substring(3, 2));
                if (1 <= mm && mm <= 12)
                {
                    try
                    {
                        birthdate = new DateTime(yy, mm, 1);
                        if (birthdate > DateTime.Today)
                        {
                            birthdate = null;
                        }
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                    }
                }
            }

            return new ParseResult(birthdate, sexe);
        }

        protected override bool IsValidChar(char ch)
            => char.IsDigit(ch) || ch == 'A' || ch == 'B';

        protected override bool OnValidate(string identification)
        {
            bool result = identification != new string('0', StandardMaxLength);

            if (result)
            {
                result = Regex.IsMatch(identification, "[0-9]{6}[0-9AB][0-9]{8}");
            }

            // sexe can contain [1,2,3,4,7,8]
            if (result)
            {
                result = int.TryParse(identification.Substring(0, 1), out int sexe) && new[] { 1, 2, 3, 4, 7, 8 }.Contains(sexe);
            }

            // month of birth [1, 12] or [62, 63] means unknown month
            if (result)
            {
                result = int.TryParse(identification.Substring(3, 2), out int month) && new[] { 1, 2, 3, 4, 7, 8, 9, 10, 11, 12, 62, 63 }.Contains(month);
            }

            // check the department and department code
            if (result)
            {
                result = CheckPlaceOfBirth(identification.Substring(5, 5));
            }

            if (result)
            {
                identification = identification.Replace("2A", "19");
                identification = identification.Replace("2B", "18");

                long number = long.Parse(identification.Substring(0, 13));
                long controlNumber = long.Parse(identification.Substring(13, 2));
                long modulo97 = number % 97;
                result = modulo97 == controlNumber;
            }

            return result;
        }

        /// <summary>
        ///     Check if the code that represent where the person is born is correct.
        /// </summary>
        /// <param name="placeOfBirth">Code that represents where the person is born.</param>
        private bool CheckPlaceOfBirth(string placeOfBirth)
            => placeOfBirth.Length == 5
               && (CheckPlaceOfBirthA(placeOfBirth)
                   || CheckPlaceOfBirthB(placeOfBirth)
                   || CheckPlaceOfBirthC(placeOfBirth));

        /// <summary>
        ///     People born in France or born in Corsica.
        /// </summary>
        /// <param name="placeOfBirth">Code that represents where the person is born.</param>
        private bool CheckPlaceOfBirthA(string placeOfBirth)
        {
            bool result;

            string departmentAsString = placeOfBirth.Substring(0, 2);
            if (int.TryParse(departmentAsString, out int department))
            {
                result = 1 <= department && department <= 19
                         || 21 <= department && department <= 95
                         || 96 <= department && department <= 99;
            }
            else
            {
                result = string.Equals(departmentAsString, "2A", StringComparison.Ordinal)
                         || string.Equals(departmentAsString, "2B", StringComparison.Ordinal);
            }

            if (result)
            {
                result = int.TryParse(placeOfBirth.Substring(2, 3), out int code) && 1 <= code && code <= 990;
            }

            return result;
        }

        /// <summary>
        ///     People not born in France, but in there colonies.
        /// </summary>
        /// <param name="placeOfBirth">Code that represents where the person is born.</param>
        private bool CheckPlaceOfBirthB(string placeOfBirth)
        {
            bool result = true;

            if (int.TryParse(placeOfBirth.Substring(0, 3), out int department))
            {
                result = 970 <= department && department <= 989;
            }

            if (result)
            {
                result = int.TryParse(placeOfBirth.Substring(3, 2), out int code) && 1 <= code && code <= 90;
            }

            return result;
        }

        /// <summary>
        ///     Persons not born in France, excluded <see cref="CheckPlaceOfBirthB" />
        /// </summary>
        /// <param name="placeOfBirth">Code that represents where the person is born.</param>
        /// <returns></returns>
        private bool CheckPlaceOfBirthC(string placeOfBirth)
        {
            bool result = true;

            if (int.TryParse(placeOfBirth.Substring(0, 2), out int department))
            {
                result = department == 99;
            }

            if (result)
            {
                result = int.TryParse(placeOfBirth.Substring(2, 3), out int code) && 1 <= code && code <= 990;
            }

            return result;
        }

        [Serializable]
        [DataContract]
        private class ParseResult
        {
            public ParseResult(DateTime? birthDate, Sexe? sexe)
            {
                BirthDate = birthDate;
                Sexe = sexe;
            }

            [DataMember]
            public DateTime? BirthDate { get; }

            [DataMember]
            public Sexe? Sexe { get; }
        }
    }
}
