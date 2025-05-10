import React from "react";

import Button from "../../common/Button";
import Input from "../../common/Input";

function LoginPage() {
  return (
    <div className="h-full w-full flex justify-center items-center">
      <div className="shadow-md w-100 rounded-md p-3 bg-white">
        <div className="flex justify-center font-medium text-xl">
          Sign in to your account
        </div>
        <div className="mt-5">Email</div>
        <Input placeholder="email@gmail.com" className="mt-1" type="email" />
        <div className="mt-5">Password</div>
        <Input placeholder="********" className="mt-1" type="password" />
        <Button className="mt-3">Login</Button>
      </div>
    </div>
  );
}

export default LoginPage;
