import * as React from "react";
import { Link } from 'react-router-dom';
import {
  Alignment,
  Classes,
  Navbar,
  NavbarGroup,
  NavbarHeading
} from "@blueprintjs/core";

export interface NavigationProps {}

export function Navigation(){
    return (
        <Navbar className={Classes.DARK}>
            <NavbarGroup align={Alignment.CENTER}>
                <NavbarHeading><Link to="/">NCloud</Link></NavbarHeading>
        </NavbarGroup>
      </Navbar>
    );
}
