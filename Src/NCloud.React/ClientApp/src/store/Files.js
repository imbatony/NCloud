"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.reducer = exports.actionCreators = exports.FileType = void 0;
// -----------------
// STATE - This defines the type of data maintained in the Redux store.
var FileType;
(function (FileType) {
    FileType[FileType["Directory"] = 0] = "Directory";
    FileType[FileType["Other"] = 1] = "Other";
})(FileType = exports.FileType || (exports.FileType = {}));
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    load: function (baseId, id) { return function (dispatch, getState) {
        // Only load data if it's something we don't already have (and are not already loading)
        var appState = getState();
        if (appState && appState.files && baseId && id) {
            console.log('id is not null');
            console.log(appState.files);
            if (baseId && id) {
                if (baseId != appState.files.baseId || id != appState.files.id) {
                    fetch("/api/file/files/" + baseId + "/" + id)
                        .then(function (resp) {
                        var message = '服务端异常';
                        try {
                            var res = resp.json();
                            return res;
                        }
                        catch (error) {
                            throw new Error(message);
                        }
                    })
                        .then(function (res) {
                        if (res.succeeded) {
                            return res;
                        }
                        else {
                            throw new Error(res.errors.toString());
                        }
                    })
                        .then(function (res) {
                        var data = res.data;
                        dispatch({ type: 'RECEIVE_FILES_INFO', baseId: data.cwd.baseId, id: data.cwd.id, children: data.children, cwd: data.cwd });
                    })
                        .catch(function (err) {
                        dispatch({ type: 'RAISE_ERROR', message: err.message });
                    });
                    dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
                }
            }
        }
        else if (appState && appState.files && (!baseId || !id)) {
            fetch("/api/file/root")
                .then(function (resp) {
                var message = '服务端异常';
                try {
                    var res = resp.json();
                    return res;
                }
                catch (error) {
                    throw new Error(message);
                }
            })
                .then(function (res) {
                if (res.succeeded) {
                    return res;
                }
                else {
                    throw new Error(res.errors.toString());
                }
            })
                .then(function (res) {
                var data = res.data;
                dispatch({ type: 'RECEIVE_FILES_INFO', baseId: data.cwd.baseId, id: data.cwd.id, children: data.children, cwd: data.cwd });
            })
                .catch(function (err) {
                dispatch({ type: 'RAISE_ERROR', message: err.message });
            })
                .catch(function (err) {
                dispatch({ type: 'RAISE_ERROR', message: err.message });
            });
            dispatch({ type: 'REQUEST_FILES_INFO', baseId: baseId, id: id });
        }
    }; },
    closeError: function () { return function (dispatch, getState) {
        dispatch({ type: 'CLOSE_ERROR' });
    }; },
    backword: function () { return function (dispatch, getState) {
        // Only load data if it's something we don't already have (and are not already loading)
        var appState = getState();
        if (appState && appState.files && appState.files.parentBaseId && appState.files.parentId) {
            var parentId_1 = appState.files.parentId;
            var parentBaseId_1 = appState.files.parentBaseId;
            fetch("/api/files/" + parentBaseId_1 + "/" + parentId_1)
                .then(function (resp) {
                var message = '服务端异常';
                try {
                    var res = resp.json();
                    return res;
                }
                catch (error) {
                    throw new Error(message);
                }
            })
                .then(function (res) {
                if (res.succeeded) {
                    return res;
                }
                else {
                    throw new Error(res.errors.toString());
                }
            })
                .then(function (res) {
                var data = res.data;
                dispatch({ type: 'RECEIVE_FILES_INFO', baseId: parentBaseId_1, id: parentId_1, children: data.children, cwd: data.cwd });
            })
                .catch(function (err) {
                dispatch({ type: 'RAISE_ERROR', message: err.message });
            });
            dispatch({ type: 'REQUEST_FILES_INFO', baseId: parentBaseId_1, id: parentId_1 });
        }
    }; }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var defaultFileState = {
    baseId: undefined,
    id: undefined,
    parentId: undefined,
    parentBaseId: undefined,
    children: [],
    cwd: undefined,
    loading: false,
    breadItems: [],
    error: false,
    errorMessage: ''
};
var reducer = function (state, incomingAction) {
    if (state === undefined) {
        return defaultFileState;
    }
    var action = incomingAction;
    switch (action.type) {
        case 'REQUEST_FILES_INFO':
            console.log('REQUEST_FILES_INFO');
            return __assign(__assign({}, state), { loading: true, baseId: action.baseId, id: action.id });
        case 'RAISE_ERROR':
            return __assign(__assign({}, state), { loading: false, error: true, errorMessage: action.message });
        case 'CLOSE_ERROR':
            return __assign(__assign({}, state), { loading: false, error: false, errorMessage: '' });
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
                    errorMessage: ''
                };
            }
            break;
    }
    return state;
};
exports.reducer = reducer;
//# sourceMappingURL=Files.js.map