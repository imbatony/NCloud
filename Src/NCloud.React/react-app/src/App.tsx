import * as React from 'react';
import { RootResponse } from './api'
import { Navigation } from './components/Navigation'
import {
  BrowserRouter as Router,
  Switch,
  Redirect,
  Route
} from "react-router-dom";
import './App.css';
import Files from './pages/Files'
import { RootContext} from './context'
export default function App(props: RootResponse) {
    return (
        <RootContext.Provider
            value={{
                rootId: props.id,
                rootBaseId: props.baseId,
            }}
        >
            <Router>
                <Navigation />
                <div>
                    <Switch>
                        <Route exact path="/" render={() => {
                            return <Redirect to={`/files/${props.baseId}/${props.id}`}></Redirect>
                        }} />
                        <Route exact path="/files/:baseId/:id"><Files/></Route>
                    </Switch>     
                </div>
            </Router>
        </RootContext.Provider>
  );
}
