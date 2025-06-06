import { EmployeePair } from "../models/employee/EmployeePair";
import { AppConfigClient } from "./utils/AppConfigClient";

const topPairsUrl = "/employees/top-pair";
const uploadUrl = "/employees/upload";

export const fetchTopPairs = async (): Promise<EmployeePair[]> => {
  const response = await AppConfigClient.get<EmployeePair[]>(topPairsUrl);
  return response.data;
};

export const uploadEmployeeFile = async (file: File): Promise<void> => {
  const formData = new FormData();
  formData.append("file", file);

  await AppConfigClient.post(uploadUrl, formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};
