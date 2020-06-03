using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OhmsLibraries.Localization {
    public interface ILanguageObtainer {
        bool HasSavedLanguage ();

        SystemLanguage GetLanguage ();
    }
}