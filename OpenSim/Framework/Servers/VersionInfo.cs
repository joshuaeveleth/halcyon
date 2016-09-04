/*
 * Copyright (c) InWorldz Halcyon Developers
 * Copyright (c) Contributors, http://opensimulator.org/
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSim Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.Reflection;

// Using a * for the third number means auto-fill with the # days since 2000. Good for us here.
// That only leaves us with two in front, so we'll divide the first number by 10.
// i.e. 9.18.R in the assembly is presented as 0.9.18.R
[assembly: AssemblyVersion("9.28.*")]
// Do not provide AssemblyFileVersion and it will be kept in sync.

namespace OpenSim
{
    using System;
    using System.IO;
    using System.Reflection;
   
    public class VersionInfo
    {
        /// <summary>
        /// This is name of the software product (separate from the grid it is running on).
        /// This should not be changed.
        /// </summary>
        public readonly static string SoftwareName = "Halcyon";
        public readonly static string SoftwareChannel = SoftwareName + " Server";

        public readonly static string DefaultGrid = "InWorldz"; // for items not overridden in [GridInfo]

        private static string _version = null;  // Change the AssemblyVersion above.
        private static string _revision = null; // Autogenerated by the build process due to '*'.

        private static string _Initialize()
        {
            if (_revision == null)
            {
                Version ver = typeof(VersionInfo).Assembly.GetName().Version;
                _version = String.Format("{0}.{1}.{2}", ver.Major/10, ver.Major % 10, ver.Minor);
                _revision = ver.Build.ToString();
            }
            return _revision;
        }


        public static string Revision
        {
            get
            {
                _Initialize();
                return _revision;
            }
        }

        /// <summary>
        /// This is the short version, such as "Halcyon 1.2.3", without revision info.
        /// </summary>
        public static string ShortVersion
        {
            get
            {
                _Initialize();
                return SoftwareName + " " + _version;
            }
        }

        /// <summary>
        /// This is the full version, with revision info, such as "Halcyon 1.2.3 R9999".
        /// This is the one requested by most of the software, and passed in RegionInfo to viewers.
        /// </summary>
        public static string FullVersion
        {
            get {
                _Initialize();
                return ShortVersion + " R" + Revision;
            }
        }

        /// <summary>
        /// This is the version value without the software name but with revision info, such as "1.2.3.9999".
        /// </summary>
        /// Mostly used by the scripting system and therefore is best to follow LL's format which seems to be "Major.Minor.Update.Revision".
        public static string Version
        {
            get {
                _Initialize();
                return _version + "." + Revision;
            }
        }

        /// <value>
        /// This is the external interface version.  It is separate from the externally-visible version info.
        /// 
        /// This version number should be increased by 1 every time a code change makes the previous revision incompatible
        /// with the new revision.  This will usually be due to interregion or grid facing interface changes.
        /// 
        /// Changes which are compatible with an older revision (e.g. older revisions experience degraded functionality
        /// but not outright failure) do not need a version number increment.
        /// 
        /// Having this version number allows the grid service to reject connections from regions running a version
        /// of the code that is too old. 
        ///   
        /// </value>
        public readonly static int MajorInterfaceVersion = 4;
    }
}
