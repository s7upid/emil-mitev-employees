import React from "react";
import { useTopPairs } from "../hooks/useTopPairs";
import EmployeeList from "../components/EmployeeList";
import FileUploader from "../components/FileUploader";
import { handleApiError } from "../utils/handleApiError";

import "./EmployeeDashboard.scss";

const EmployeeDashboard: React.FC = () => {
  const { data, isLoading, error, refetch } = useTopPairs();

  return (
    <div className="employee-dashboard">
      <h2>Top Collaborating Employees</h2>

      <FileUploader onUploadSuccess={refetch} />

      {isLoading && <p className="loading">Loading...</p>}
      {error && <p className="error">{handleApiError(error)}</p>}
      {!isLoading && !error && <EmployeeList pairs={data || []} />}
    </div>
  );
};

export default EmployeeDashboard;
