import { zodResolver } from "@hookform/resolvers/zod";
import {
  IconBrandFacebookFilled,
  IconBrandGoogleFilled,
} from "@tabler/icons-react";
import React from "react";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import { z } from "zod";

import { login } from "@/apis/oauth";
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
import dataConfigs from "@/configs/dataConfigs";
import { ResultStatus } from "@/enums/ResultStatus";

function LoginPage(): React.JSX.Element {
  const formSchema = z.object({
    email: z.string().nonempty().email().max(dataConfigs.DefaultString),
    password: z
      .string()
      .nonempty()
      .min(dataConfigs.PasswordMinLength)
      .max(dataConfigs.DefaultString),
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
        break;
      case ResultStatus.NotFound:
        break;
      case ResultStatus.IsNotAllowed:
        break;
      case ResultStatus.IsLockedOut:
        break;
      case ResultStatus.Failure:
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
              <IconBrandGoogleFilled color="red" />
            </Link>
            <Link to="/" className="rounded-full p-1 cursor-pointer">
              <IconBrandFacebookFilled color="blue" />
            </Link>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

export default LoginPage;
