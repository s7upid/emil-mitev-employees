import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import FileUploader from "./FileUploader";
import { AppConfigClient } from "../../api/utils/AppConfigClient";

jest.mock("../../api/utils/AppConfigClient", () => ({
  AppConfigClient: {
    post: jest.fn(),
  },
}));

describe("FileUploader", () => {
  it("should upload a file and call onUploadSuccess", async () => {
    const mockSuccess = jest.fn();
    (AppConfigClient.post as jest.Mock).mockResolvedValue({ status: 200 });

    render(<FileUploader onUploadSuccess={mockSuccess} />);

    const file = new File(["name,email\nJohn,john@example.com"], "test.csv", {
      type: "text/csv",
    });

    const input = screen.getByTestId("file-input");
    fireEvent.change(input, { target: { files: [file] } });

    fireEvent.click(screen.getByTestId("upload-button"));

    await waitFor(() => {
      expect(mockSuccess).toHaveBeenCalled();
    });

    expect(screen.getByText("Upload successful")).toBeInTheDocument();
  });
});
