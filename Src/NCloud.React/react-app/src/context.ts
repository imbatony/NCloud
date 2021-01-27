import { createContext } from "react";
import { RootResponse } from './api'
export const RootContext = createContext<RootResponse>({
    rootBaseId: '',
    rootId: '',
    name: "NCloud",
    version: "1.0.0",
    buildId: "0"
})