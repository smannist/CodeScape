import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
} from "@mui/material";

import { blue } from "@mui/material/colors";

interface Props {
  returns: string | undefined;
}

const FunctionReturnsTable = ({ returns }: Props) => (
  <TableContainer component={Paper} sx={{ mt: 1 }}>
    <Table>
      <TableHead>
        <TableRow sx={{ backgroundColor: blue[100] }}>
          <TableCell
            sx={{
              position: "sticky",
              top: 0,
              backgroundColor: blue[100],
              width: "50%",
            }}
          >
            <strong>Description</strong>
          </TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        <TableRow>
          <TableCell>{returns}</TableCell>
        </TableRow>
      </TableBody>
    </Table>
  </TableContainer>
);

export default FunctionReturnsTable;
