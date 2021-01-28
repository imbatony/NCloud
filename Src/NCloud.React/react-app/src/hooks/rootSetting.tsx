import { createContext, useContext, useEffect, useState } from "react";
import { RootConfig } from '../types'
import {
    Classes
} from "@blueprintjs/core";
interface RootSettings extends RootConfig {
    dark: boolean
    setDark: (b: boolean) => void
}
const RootContext = createContext<RootSettings>({
    rootBaseId: '',
    rootId: '',
    name: "NCloud",
    version: "1.0.0",
    buildId: "0",
    dark: false,
    setDark: (b: boolean) => { }
})
interface RootContextProviderProps {
    rootConfig: RootConfig
    children: JSX.Element
}
export const RootContextProvider = ({ rootConfig, children }: RootContextProviderProps) => {
    const [dark, setDarkTheme] = useState<boolean>(false);
    useEffect(()=> {
            document.body.className = dark?Classes.DARK:''
    }, [dark])
    return (
        <RootContext.Provider value={{ ...rootConfig, dark, setDark: setDarkTheme }}>
                {children}
        </RootContext.Provider>
    )
}

export const useRootContext = (): RootSettings => useContext<RootSettings>(RootContext);
