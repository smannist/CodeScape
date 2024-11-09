import { Link as RouterLink } from "react-router-dom";

import { List, ListItemButton, ListItemText, Typography } from "@mui/material";

import { ClassDescription } from "../types/description";

interface Props {
  classes: ClassDescription[];
}

const ClassList = ({ classes }: Props): JSX.Element => {
  return (
    <List component="div" disablePadding>
      <Typography variant="subtitle1" sx={{ color: "gray", mt: 1 }}>
        Classes
      </Typography>
      {classes.map((cls, idx) => (
        <ListItemButton
          key={idx}
          component={RouterLink}
          to={`/class/${cls.name}`}
          sx={{ pl: 1 }}
        >
          <ListItemText primary={cls.name} />
        </ListItemButton>
      ))}
    </List>
  );
};

export default ClassList;
