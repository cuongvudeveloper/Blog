import { ResultStatus } from "@/enums/ResultStatus";

export interface Result<T extends object> {
  succeeded: boolean;
  errors: string[];
  data?: T;
  status: ResultStatus;
}
