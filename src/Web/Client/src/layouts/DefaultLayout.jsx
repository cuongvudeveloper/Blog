import React from "react";
import { Link } from "react-router-dom";

import logo from "../assets/logo.png";
import Button from "../common/Button";
import menu from "../configs/menu";

function DefaultLayout() {
  return (
    <header className="h-15 shadow-md">
      <div className="flex justify-between items-center h-full p-3">
        <img src={logo} alt="Hanover" className="h-full" />
        <div>
          {menu.map((item, index) => (
            <Link
              key={index}
              to={item.to}
              className="font-medium hover:text-blue-500 mx-6"
            >
              {item.title}
            </Link>
          ))}
        </div>
        <div className="h-full flex items-center gap-3">
          <Link className="mx-1 h-full" to="/register">
            <Button>Sign up</Button>
          </Link>
          <Link className="mx-1 h-full" to="/login">
            <Button>Login</Button>
          </Link>
        </div>
      </div>
    </header>
  );
}

export default DefaultLayout;
