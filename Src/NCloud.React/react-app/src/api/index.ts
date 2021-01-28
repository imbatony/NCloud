import { RestResult, RootConfig, ListFileInfo, defaultLocal } from '../types'
function get<T>(url: string, mockData?: T): Promise<T> {
    const lang = localStorage.getItem('local') || defaultLocal;
    const requestHeaders: HeadersInit = new Headers();
    requestHeaders.set('accept-language', lang);
    if (mockData) {
        return Promise.resolve(mockData);
    } else {
        const message = '服务端访问失败';
        return new Promise(function (resolve, reject) {
            fetch(new Request(url, {
                headers: requestHeaders
            }))
                .then((resp) => (resp.json() as Promise<RestResult<T>>))
                .then((res) => {
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
                .catch(err => {
                    reject(err.message || message)
                })
        })
    }
}

export async function getRoot(): Promise<RootConfig> {
    return await get<RootConfig>('/api/system')
}
export function getFiles(baseId: string, id: string) {
    return get<ListFileInfo>(`/api/file/files/${baseId}/${id}`);
}