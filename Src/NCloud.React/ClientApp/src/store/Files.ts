import { Action, Reducer } from 'redux';
import { AppThunkAction, RestResult } from './';
// -----------------
// STATE - This defines the type of data maintained in the Redux store.
enum FileType {
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
    loading: boolean
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.
// Use @typeName and isActionType for type detection that works even after serialization/deserialization.

export interface IncrementCountAction { type: 'INCREMENT_COUNT' }
export interface DecrementCountAction { type: 'DECREMENT_COUNT' }

export interface RequestFilesAction {
    type: 'REQUEST_FILES_INFO';
    baseId: string | undefined;
    id: string | undefined;
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
export type KnownAction = RequestFilesAction | ReceiveFilesAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    load: (baseId: string | undefined, id: string | undefined): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.files && baseId && id) {
            if (baseId != null && baseId != appState.files.baseId && id != null && id != appState.files.id) {
                fetch(`/api/file/files/${baseId}/${id}`)
                    .then(response => response.json() as Promise<FilesRestResult>)
                    .then(res => {
                        const data = res.data;
                        dispatch({ type: 'RECEIVE_FILES_INFO', baseId: baseId, id: id, children: data.children, cwd: data.cwd });
                    });

                dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
            }          
        } else if (appState && appState.files && (!baseId || !id)) {
            fetch(`/api/file/root`)
                .then(response => response.json() as Promise<FilesRestResult>)
                .then(res => {
                    const data = res.data;
                    dispatch({ type: 'RECEIVE_FILES_INFO', baseId: data.cwd.baseId, id: data.cwd.id, children: data.children, cwd: data.cwd });
                });

            dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
        }
    },
    backword: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.files && appState.files.parentBaseId  && appState.files.parentId) {
            const parentId = appState.files.parentId;
            const parentBaseId = appState.files.parentBaseId;
            fetch(`/api/files/${parentBaseId}/${parentId}`)
                .then(response => response.json() as Promise<FilesRestResult>)
                .then(res => {
                    const data = res.data;
                    dispatch({ type: 'RECEIVE_FILES_INFO', baseId: parentBaseId, id: parentId, children: data.children, cwd: data.cwd });
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
    loading:false
}
export const reducer: Reducer<FilesState> = (state: FilesState | undefined, incomingAction: Action): FilesState => {
    if (state === undefined) {
        return defaultFileState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_FILES_INFO':
            return { ...state, loading: true, baseId: action.baseId, id: action.id };
        case 'RECEIVE_FILES_INFO':
            if ((!state.baseId || state.baseId == action.baseId) && (!state.id || state.id == action.id)) {
                return {
                    baseId: action.baseId,
                    id: action.id,
                    parentId: action.cwd.parentId,
                    parentBaseId: action.cwd.parentBaseId,
                    children: action.children,
                    cwd: action.cwd,
                    loading: false
                };
            }
            break;         
    }
    return state;
};
