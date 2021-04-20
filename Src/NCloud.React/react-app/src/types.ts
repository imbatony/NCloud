export interface StringDictionary {
    [index: string]: string;
}
export interface RestResult<T> {
    statusCode: number;
    succeeded: boolean;
    errors: string | Array<String>;
    extras: Object;
    timestamp: number;
    data: T
}
export interface RootConfig {
    name: string,
    buildId: string,
    version: string,
    rootBaseId: string,
    rootId: string
}

export enum FileType {
    Directory = 0,
    Other,
    Link
}
export interface FileInfo {
    name: string;
    baseId: string;
    id: string;
    parentId: string;
    parentBaseId: string;
    ext: string;
    icon: string;
    size: number;
    type: FileType;
    updateTime: Date;
    createTime: Date;
}
export interface ListFileInfo {
    cwd: FileInfo;
    children: Array<FileInfo>;
}

export const defaultLocal = 'zh-CN'