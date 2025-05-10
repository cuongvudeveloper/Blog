import React from "react";

import { login } from "../../apis/oauth";
import Button from "../../common/Button";
import Input from "../../common/Input";

function LoginPage() {
  const [email, setEmail] = React.useState("system@gmail.com");
  const [password, setPassword] = React.useState("System1!");

  const loginHandler = async () => {
    await login(email, password);
  };

  return (
    <div className="h-full w-full flex justify-center items-center">
      <div className="shadow-md w-100 rounded-md p-3 bg-white">
        <div className="flex justify-center font-medium text-xl">
          Sign in to your account
        </div>
        <div className="mt-5">Email</div>
        <Input
          value={email}
          placeholder="email@gmail.com"
          className="mt-1"
          type="email"
          onChange={(e) => {
            setEmail(e.target.value);
          }}
        />
        <div className="mt-5">Password</div>
        <Input
          value={password}
          placeholder="********"
          className="mt-1"
          type="password"
          onChange={(e) => {
            setPassword(e.target.value);
          }}
        />
        <Button className="mt-5" onClick={loginHandler}>
          Login
        </Button>
      </div>
    </div>
  );
}

export default LoginPage;
