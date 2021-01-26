export interface RestResult<T> {
    statusCode: number;
    succeeded: boolean;
    errors: string | Array<String>;
    extras: Object;
    timestamp: number;
    data: T
}
 
function get<T>(url: string, mockData?: T): Promise<T> {
    if (mockData) {
        return Promise.resolve(mockData);
    } else {
        const message = '服务端访问失败';
        return new Promise(function(resolve,reject){
            fetch(url)
            .then((resp)=> (resp.json() as Promise<RestResult<T>>))
            .then((res)=>{
                if (res.succeeded) {
                    resolve(res.data);
                }
                else if (!res.errors) {
                    throw new Error(message);
                } else {
                    if (typeof res.errors === 'string') {
                        console.log(res.errors)
                        throw new Error(res.errors.toString());
                    } else {
                        const err = (res.errors as Array<string>)[0];
                        throw new Error(err);
                    }
                }
            })
            .catch(err=>{
                reject(err.message||message)
            })
        })            
    }
}
export interface RootResponse {
    baseId: string,
    id: string
}
export async function getRoot(): Promise<RootResponse> {
    return await get<RootResponse>('/api/file/root')
}

export enum FileType {
    Directory = 0,
    Other,
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

export function getFiles(baseId: string, id: string) {
    return get<ListFileInfo>(`/api/file/files/${baseId}/${id}`);
}