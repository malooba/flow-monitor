//Copyright 2016 Malooba Ltd

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Remote
{
    public static class VersionUtils
    {
        /// <summary>
        /// Convert a version string to a long integer.
        /// version strings may have up to 4 segments where each segment may
        /// be 0-9999.
        /// missing segements are assumed to be zero (1.0 == 1.0.0.0)
        /// </summary>
        /// <param name="version">version string</param>
        /// <returns>long integer version</returns>
        public static long ConvertVersion(string version)
        {
            var vs = version.Split('.');

            if(vs.Length > 4)
                throw new ApplicationException("Too many version numbers");

            return vs
                .Select(long.Parse)
                .Concat(Enumerable.Repeat(0L, 4 - vs.Length))
                .Aggregate((acc, v) =>
                {
                    if(v > 9999) throw new ApplicationException("version number >= 10000");
                    return acc * 10000 + v;
                });
        }

        /// <summary>
        /// Convert a long integer to a version string.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string ConvertVersion(long version)
        {
            var vv = version;
            var parts = new List<string>(4);
            for(var i = 0; i < 4; i++)
            {
                var v = vv % 10000;
                parts.Add(v.ToString());
                vv /= 10000;
            }
            if(vv != 0)
                throw new ApplicationException("Invalid version number");
            parts.Reverse();
            return string.Join(".", parts);
        }

        /// <summary>
        /// Normalise a version string by converting twice
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string NormaliseVersion(string version)
        {
            return ConvertVersion(ConvertVersion(version));
        }
    }
}
