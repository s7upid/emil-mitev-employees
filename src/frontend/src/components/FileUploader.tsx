import React, { useState } from "react";
import { uploadEmployeeFile } from "../api/employees";

import "./FileUploader.scss";

interface Props {
  onUploadSuccess: () => void;
}

const FileUploader: React.FC<Props> = ({ onUploadSuccess }) => {
  const [file, setFile] = useState<File | null>(null);
  const [error, setError] = useState("");

  const handleUpload = async () => {
    if (!file) {
      setError("Please select a file.");
      return;
    }

    try {
      await uploadEmployeeFile(file);
      onUploadSuccess();
      setError("");
    } catch (err: any) {
      setError("Upload failed. Please try again.");
    }
  };

  return (
    <div className="file-uploader">
      <input
        type="file"
        accept=".csv"
        onChange={(e) => setFile(e.target.files?.[0] || null)}
      />
      <button onClick={handleUpload}>Upload</button>
      {error && <p className="error-message">{error}</p>}
    </div>
  );
};

export default FileUploader;
