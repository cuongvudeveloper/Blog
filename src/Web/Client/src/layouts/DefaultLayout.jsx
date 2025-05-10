import React from "react";
import { Link } from "react-router-dom";

import logo from "../assets/logo.png";
import menu from "../configs/menu";

function DefaultLayout() {
  return (
    <>
      <header className="h-15 shadow-md">
        <div className="flex justify-between items-center h-full p-3">
          <img src={logo} alt="Hanover" className="h-full" />
          <div>
            {menu.map((item, index) => (
              <Link
                key={index}
                to={item.to}
                className="hover:text-blue-500 px-5"
              >
                {item.title}
              </Link>
            ))}
          </div>
          <div className="h-full flex items-center gap-3">
            <button className="px-3 h-full rounded-[5px] cursor-pointer bg-black text-white hover:bg-gray-200 hover:text-black">
              Sign up
            </button>
            <button className="px-3 h-full rounded-[5px] cursor-pointer bg-black text-white hover:bg-gray-200 hover:text-black">
              Login
            </button>
          </div>
        </div>
      </header>
    </>
  );
}

export default DefaultLayout;
