import { IBreadcrumbProps } from '@blueprintjs/core';
import { Action, Reducer } from 'redux';
import { AppThunkAction, RestResult } from './';
// -----------------
// STATE - This defines the type of data maintained in the Redux store.
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

export interface FilesRestResult extends RestResult {
    data: ListFileInfo;
}

export interface ListFileInfo extends RestResult {
    cwd: FileInfo;
    children: Array<FileInfo>;
}

export interface FilesState {
    baseId: string | undefined;
    id: string | undefined;
    parentId: string | undefined;
    parentBaseId: string | undefined;
    children: Array<FileInfo>;
    cwd: FileInfo | undefined,
    loading: boolean,
    breadItems: Array<IBreadcrumbProps>,
    error: boolean,
    errorMessage: string
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface RequestFilesAction {
    type: 'REQUEST_FILES_INFO';
    baseId: string | undefined;
    id: string | undefined;
}
export interface CloseErrorAction {
    type: 'CLOSE_ERROR';
}
export interface RaiseErrorAction {
    type: 'RAISE_ERROR';
    message:string
}
export interface ReceiveFilesAction {
    type: 'RECEIVE_FILES_INFO';
    baseId: string ;
    id: string ;
    children: FileInfo[];
    cwd: FileInfo;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = RequestFilesAction | ReceiveFilesAction | CloseErrorAction | RaiseErrorAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    load: (baseId: string | undefined, id: string | undefined): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.files && baseId && id) {
            console.log('id is not null');
            console.log(appState.files)
            if (baseId && id) {
                if (baseId != appState.files.baseId || id != appState.files.id) {
                    fetch(`/api/file/files/${baseId}/${id}`)
                        .then(resp => {

                            let message = '服务端异常';
                            try {
                                let res = resp.json() as Promise<FilesRestResult>
                                return res;
                            }
                            catch (error) {
                                throw new Error(message);
                            }

                        })
                        .then(res => {
                            if (res.succeeded) {
                                return res;
                            } else {
                                throw new Error(res.errors.toString())
                            }
                        })
                        .then(res => {
                            const data = res.data;
                            dispatch({ type: 'RECEIVE_FILES_INFO', baseId: data.cwd.baseId, id: data.cwd.id, children: data.children, cwd: data.cwd });
                        })
                        .catch(err => {
                            dispatch({ type: 'RAISE_ERROR', message: err.message })
                        });

                    dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
                }          
            }          
        } else if (appState && appState.files && (!baseId || !id)) {
            fetch(`/api/file/root`)
                .then(resp => {

                        let message = '服务端异常';
                        try {
                            let res = resp.json() as Promise<FilesRestResult>
                            return res;
                        }
                        catch (error) {
                            throw new Error(message);
                        }                     
                                     
                })
                .then(res => {
                    if (res.succeeded) {
                        return res;
                    } else {
                        throw new Error(res.errors.toString())
                    }
                })
                .then(res => {
                    const data = res.data;
                    dispatch({ type: 'RECEIVE_FILES_INFO', baseId: data.cwd.baseId, id: data.cwd.id, children: data.children, cwd: data.cwd });
                })
                .catch(err => {
                    dispatch({ type: 'RAISE_ERROR', message:err.message })
                })
                .catch(err => {
                    dispatch({ type: 'RAISE_ERROR', message: err.message })
                });

            dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
        }
    },
    closeError: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: 'CLOSE_ERROR'});
    },
    backword: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.files && appState.files.parentBaseId  && appState.files.parentId) {
            const parentId = appState.files.parentId;
            const parentBaseId = appState.files.parentBaseId;
            fetch(`/api/files/${parentBaseId}/${parentId}`)
                .then(resp => {

                    let message = '服务端异常';
                    try {
                        let res = resp.json() as Promise<FilesRestResult>
                        return res;
                    }
                    catch (error) {
                        throw new Error(message);
                    }

                })
                .then(res => {
                    if (res.succeeded) {
                        return res;
                    } else {
                        throw new Error(res.errors.toString())
                    }
                })
                .then(res => {
                    const data = res.data;
                    dispatch({ type: 'RECEIVE_FILES_INFO', baseId: parentBaseId, id: parentId, children: data.children, cwd: data.cwd });
                })
                .catch(err => {
                    dispatch({ type: 'RAISE_ERROR', message: err.message })
                });

            dispatch({ type: 'REQUEST_FILES_INFO', baseId: parentBaseId, id: parentId });
        }
    }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const defaultFileState: FilesState = {
    baseId: undefined,
    id: undefined,
    parentId: undefined,
    parentBaseId: undefined,
    children: [],
    cwd: undefined,
    loading: false,
    breadItems: [],
    error: false,
    errorMessage:''
}
export const reducer: Reducer<FilesState> = (state: FilesState | undefined, incomingAction: Action): FilesState => {
    if (state === undefined) {
        return defaultFileState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_FILES_INFO':
            console.log('REQUEST_FILES_INFO');
            return { ...state, loading: true, baseId: action.baseId, id: action.id };
        case 'RAISE_ERROR':
            return { ...state, loading: false, error: true, errorMessage: action.message };
        case 'CLOSE_ERROR':
            return { ...state, loading: false, error: false, errorMessage: '' };
        case 'RECEIVE_FILES_INFO':
            console.log('RECEIVE_FILES_INFO');
            if ((!state.baseId || state.baseId == action.baseId) && (!state.id || state.id == action.id)) {
                return {
                    baseId: action.baseId,
                    id: action.id,
                    parentId: action.cwd.parentId,
                    parentBaseId: action.cwd.parentBaseId,
                    children: action.children,
                    cwd: action.cwd,
                    loading: false,
                    breadItems: [{ icon: "folder-open", text: action.cwd.name, current: true }],
                    error: false,
                    errorMessage:''
                };
            }
            break;         
    }
    return state;
};
