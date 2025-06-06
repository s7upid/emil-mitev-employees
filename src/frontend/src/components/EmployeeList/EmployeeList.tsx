import React from "react";
import { EmployeePair } from "../../models/employee/EmployeePair";

import "./EmployeeList.scss";

type Props = { pairs: EmployeePair[] };

const EmployeeList: React.FC<Props> = ({ pairs }) => {
  if (!pairs || pairs.length === 0) {
    return <p>No employee pairs found.</p>;
  }

  return (
    <div className="employee-list">
      <table>
        <thead>
          <tr>
            <th>Employee 1</th>
            <th>Employee 2</th>
            <th>Total Days Worked</th>
          </tr>
        </thead>
        <tbody>
          {pairs.map((pair, i) => (
            <tr key={i}>
              <td>{pair.employeeId1}</td>
              <td>{pair.employeeId2}</td>
              <td>{pair.totalDaysWorked}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default EmployeeList;
