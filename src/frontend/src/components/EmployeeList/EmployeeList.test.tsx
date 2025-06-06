import React from "react";
import { render, screen } from "@testing-library/react";
import EmployeeList from "./EmployeeList";
import { EmployeePair } from "../../models/employee/EmployeePair";

const mockData: EmployeePair[] = [
  { employeeId1: "101", employeeId2: "202", totalDaysWorked: 120 },
  { employeeId1: "301", employeeId2: "402", totalDaysWorked: 80 },
];

describe("EmployeeList", () => {
  it("renders employee pairs in a table", () => {
    render(<EmployeeList pairs={mockData} />);
    expect(screen.getByText("Employee 1")).toBeInTheDocument();
    expect(screen.getByText("101")).toBeInTheDocument();
    expect(screen.getByText("80")).toBeInTheDocument();
  });

  it("renders message when list is empty", () => {
    render(<EmployeeList pairs={[]} />);
    expect(screen.getByText("No employee pairs found.")).toBeInTheDocument();
  });
});
