import { ResultStatus } from "@/enums/result-status";

export type Result<T extends object> = {
  succeeded: boolean;
  errors: string[];
  data?: T;
  status: ResultStatus;
};
