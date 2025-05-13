import type { Result } from "@/apis/types";
import axiosInstance from "@/plugins/axiosInstance";

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
}

export async function login(data: LoginRequest) {
  try {
    const response = await axiosInstance.post<Result<LoginResponse>>(
      "oauth/login",
      data
    );
    return response.data;
  } catch (error) {
    console.error(error);
  }
}
