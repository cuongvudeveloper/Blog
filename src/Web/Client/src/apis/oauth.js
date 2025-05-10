import axiosInstance from "../plugins/axiosInstance";

export async function login(email, password) {
  const response = await axiosInstance.post("Oauth/login", { email, password });
  return response;
}
