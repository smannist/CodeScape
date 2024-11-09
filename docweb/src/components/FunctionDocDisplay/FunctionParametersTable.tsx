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
  params: { name: string; description: string }[];
}

const FunctionParametersTable = ({ params }: Props) => (
  <TableContainer component={Paper} sx={{ mt: 2, mb: 2 }}>
    <Table stickyHeader>
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
            <strong>Name</strong>
          </TableCell>
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
        {params.map((param, idx) => (
          <TableRow key={idx}>
            <TableCell>{param.name}</TableCell>
            <TableCell>{param.description}</TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  </TableContainer>
);

export default FunctionParametersTable;
