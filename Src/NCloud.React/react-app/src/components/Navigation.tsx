import * as React from "react";
import { Link } from 'react-router-dom';
import {
  Alignment,
  Button,
  Navbar,
  NavbarGroup,
  ButtonGroup,
  NavbarHeading
} from "@blueprintjs/core";
import { useI18n, useRootContext } from '../hooks'

export interface NavigationProps { }

export function Navigation() {
  const root = useRootContext();
  const {message} = useI18n()
  return (
    <Navbar>
      <NavbarGroup align={Alignment.LEFT}>
        <NavbarHeading style={{ margin: "0 auto" }}>NCloud<small style={{ fontSize: 10, marginLeft: 10 }}>{`${root.version}.${root.buildId}`}</small></NavbarHeading>
      </NavbarGroup>
      <NavbarGroup align={Alignment.RIGHT}>
        <ButtonGroup style={{ minWidth: 200 }}>
          <Link to="/"><Button className="bp3-minimal" icon="home" text={message["home"]} ></Button></Link>
          {/*root.dark ? <Button className="bp3-minimal" icon="moon" onClick={() => { root.setDark(false) }}>{message["light"]}</Button> : <Button className="bp3-minimal" icon="flash" onClick={() => { root.setDark(true) }}>{message["dark"]}</Button>*/}
          <Button className="bp3-minimal" icon="cog">{message["settings"]}</Button>
        </ButtonGroup>
      </NavbarGroup>
    </Navbar>
  );
}
