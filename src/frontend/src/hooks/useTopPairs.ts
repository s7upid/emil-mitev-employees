import { useQuery } from "@tanstack/react-query";
import { fetchTopPairs } from "../api/employees";
import { EmployeePair } from "../models/employee/EmployeePair";

export const useTopPairs = () => {
  return useQuery<EmployeePair[], Error>({
    queryKey: ["topPairs"],
    queryFn: fetchTopPairs,
    refetchOnMount: false,
    staleTime: 5 * 60 * 1000,
    retry: 1,
  });
};
