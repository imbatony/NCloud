import * as React from "react";
import { Link } from 'react-router-dom';
import {
  Alignment,
  Button,
  Navbar,
  NavbarGroup,
  NavbarHeading
} from "@blueprintjs/core";
import {RootContext} from '../context'

export interface NavigationProps { }

export function Navigation() {
  const root = React.useContext(RootContext)
  return (
    <Navbar>
      <NavbarGroup align={Alignment.LEFT}>
        <NavbarHeading style={{margin:"0 auto"}}>NCloud<small style={{fontSize:10,marginLeft:10}}>{`${root.version}.${root.buildId}`}</small></NavbarHeading>
      </NavbarGroup>
      <NavbarGroup align={Alignment.RIGHT}>
      <Link to="/"><Button className="bp3-minimal" icon="home" text="Home" ></Button></Link>
      <Navbar.Divider />
      <Button className="bp3-minimal" icon="cog"/>
      </NavbarGroup>
    </Navbar>
  );
}
