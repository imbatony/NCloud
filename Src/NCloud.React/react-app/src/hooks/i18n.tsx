import React, { createContext, useEffect, useContext } from 'react';
import { useLocalStorage } from './useLocalstorage'
import { StringDictionary, defaultLocal } from '../types'
export interface Locales {
    date: number,
    lang: string,
    data: StringDictionary
}
export interface TransProvider {
    i18n: Map<string, StringDictionary>,
    children: JSX.Element
}
interface ContextType {
    getMessages: () => StringDictionary,
    lang: string,
    setNewLang: (s: string) => void,
    locales?: Locales
}
let defaultContextType: ContextType = { getMessages: () => { return {} }, setNewLang: (l: string) => { }, lang: defaultLocal };
const Context = createContext<ContextType>(defaultContextType);
export const TransProvider = ({ i18n, children }: TransProvider) => {
    if (!i18n) {
        throw new Error('No i18n provide.');
    }
    const [lang, setLang] = useLocalStorage<string>('lang', defaultLocal);
    const [locales, setLocales] = useLocalStorage<Locales>('locales');

    useEffect(() => {
        // load lang
        let currentLang = lang;
        if (!lang) {
            const { navigator } = window;
            if (navigator) {
                const { language, languages } = navigator;
                currentLang =
                    language || (languages && languages.length && languages[0]) || defaultLocal;
            }
            setLang(currentLang);
            return;
        }

        // load locales
        if (
            !locales ||
            !locales.date ||
            !locales.lang ||
            locales.lang !== currentLang ||
            Date.now() - locales.date > 86400000
        ) {
            console.log(currentLang);
            console.log(i18n);
            setLocales({ data: i18n.get(currentLang) || {}, lang: currentLang, date: Date.now() });
        }
    });

    const getMessages = () => locales.data;

    const setNewLang = (newLang: string) => {
        setLang(newLang);
        setLocales({ data: i18n.get(newLang) || {}, lang: newLang, date: Date.now() });
    };

    if (!locales) return <div>loading…</div>;
    return (
        <Context.Provider
            value={{
                lang,
                locales,
                getMessages,
                setNewLang,
            }}
        >
            {children}
        </Context.Provider>
    );
};

export const useI18n = () => {
    const { getMessages, lang, setNewLang } = useContext(Context);
    return { message: getMessages(), lang, setLang: setNewLang };
};
