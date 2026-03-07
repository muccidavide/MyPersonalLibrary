const ROLES = {
  USER: 'User',
  ADMIN: 'Admin'
};

const PERMISSIONS = {
  [ROLES.EDITOR]: ['read', 'write'],            
  [ROLES.ADMIN]: ['read', 'write', 'delete'] 
};

export default {
  ROLES,
  PERMISSIONS
};
