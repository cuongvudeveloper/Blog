import type { Result } from "@/apis/types";
import axiosInstance from "@/plugins/axiosInstance";

export type LoginRequest = {
  email: string;
  password: string;
};

export type LoginResponse = {
  accessToken: string;
};

export type InfoResponse = {
  userName: string;
  email: string;
  roles: string[];
};

export async function login(data: LoginRequest) {
  const response = await axiosInstance.post<Result<LoginResponse>>(
    "oauth/login",
    data
  );
  return response.data;
}

export async function info() {
  const response = await axiosInstance.get<Result<InfoResponse>>("oauth/info");
  return response.data;
}
