﻿import * as React from "react";
import {
  Alignment,
  AnchorButton,
  Classes,
  Navbar,
  NavbarGroup,
  NavbarHeading,
  NavbarDivider
} from "@blueprintjs/core";

export interface NavigationProps {}

export class Navigation extends React.PureComponent<NavigationProps> {
  public render() {
    return (
        <Navbar className={Classes.DARK}>
            <NavbarGroup align={Alignment.CENTER}>
          <NavbarHeading>NCloud</NavbarHeading>
        </NavbarGroup>
      </Navbar>
    );
  }
}