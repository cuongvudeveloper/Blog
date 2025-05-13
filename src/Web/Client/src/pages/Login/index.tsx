import { zodResolver } from "@hookform/resolvers/zod";
import { IconCheck, IconExclamationCircle } from "@tabler/icons-react";
import React from "react";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import { toast } from "sonner";
import { z } from "zod";

import { login } from "@/apis/oauth";
import FacebookLogo from "@/assets/img/facebook.webp";
import GoogleLogo from "@/assets/img/google.webp";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { DataConfigs } from "@/configs/data-configs";
import { ResultStatus } from "@/enums/result-status";

function LoginPage(): React.JSX.Element {
  const formSchema = z.object({
    email: z.string().nonempty().email().max(DataConfigs.DefaultString),
    password: z
      .string()
      .nonempty()
      .min(DataConfigs.PasswordMinLength)
      .max(DataConfigs.DefaultString),
  });

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      email: "system@gmail.com",
      password: "System1!",
    },
  });

  async function onSubmit(values: z.infer<typeof formSchema>) {
    const response = await login({
      email: values.email,
      password: values.password,
    });
    switch (response?.status) {
      case ResultStatus.Success:
        localStorage.setItem(
          import.meta.env.VITE_ACCESS_TOKEN_KEY_NAME,
          response.data!.accessToken
        );
        toast.success("Success", {
          description: "Log in successfully.",
          icon: <IconCheck />,
        });
        break;
      case ResultStatus.NotFound:
        toast.error("Failure", {
          description: "Your account does not exist.",
          icon: <IconExclamationCircle />,
        });
        break;
      case ResultStatus.IsNotAllowed:
        toast.error("Failure", {
          description: "Your account has not been verified.",
          icon: <IconExclamationCircle />,
        });
        break;
      case ResultStatus.IsLockedOut:
        toast.error("Failure", {
          description: "Your account has been locked. Please try again later.",
          icon: <IconExclamationCircle />,
        });
        break;
      case ResultStatus.Failure:
        toast.error("Failure", {
          description: "Password is incorrect.",
          icon: <IconExclamationCircle />,
        });
        break;
    }
  }

  return (
    <div className="h-full w-full flex items-center justify-center">
      <Card className="w-[400px] shadow-2xl">
        <CardHeader>
          <CardTitle className="text-xl text-center">
            Sign in to your account
          </CardTitle>
        </CardHeader>
        <CardContent>
          <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-5">
              <FormField
                control={form.control}
                name="email"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Email</FormLabel>
                    <FormControl>
                      <Input placeholder="Email" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="password"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Password</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Password"
                        {...field}
                        type="password"
                      />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <Button type="submit" className="w-full">
                Login
              </Button>
            </form>
          </Form>
          <Label className="flex justify-center mt-3">
            Don&apos;t have an account?{" "}
            <Link to="/register" className="text-blue-500">
              Sign up
            </Link>
          </Label>
          <div className="flex justify-center gap-2 mt-3">
            <Link to="/" className="rounded-full p-1 cursor-pointer">
              <img src={FacebookLogo} alt="Facebook" className="w-6" />
            </Link>
            <Link to="/" className="rounded-full p-1 cursor-pointer">
              <img src={GoogleLogo} alt="Google" className="w-6" />
            </Link>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

export default LoginPage;
